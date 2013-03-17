
#include "stdafx.h"
#using <mscorlib.dll>
#include <fstream>
#include <windows.h>
#include <tchar.h>
#include <stdio.h>
#include <strsafe.h>
#include <vcclr.h> 


using namespace System;

//=0x80000000L read
//=0x40000000L write

namespace NativeHelpers
{


[Flags]
public enum class FileOpenType : unsigned int  { GENERICREAD=0x80000000L,GENERICWRITE=0x40000000L };

public enum class MoveMethod {FILEBEGIN=0,FILECURRENT=1,FILEEND=2};

public enum class CreationDisposition {CREATENEW=1,CREATEALWAYS=2,OPENEXISTING=3,OPENALWAYS=4,TRUNCATEEXISTING=5};



public ref class NativeFile 
{
private:
	HANDLE _hFile; //unmanaged resource
	System::String ^_fileName;

public:
  NativeFile(System::String ^fileName, FileOpenType openType, CreationDisposition creation)
  {
	  _fileName=fileName;
	  pin_ptr<const wchar_t> wname = PtrToStringChars(fileName); 



	  _hFile = CreateFile(wname,                // name of the write
                       (DWORD)openType,          // open for writing
                       0,                      // do not share
                       NULL,                   // default security
                       (DWORD)creation,             // create new file only
                       FILE_ATTRIBUTE_NORMAL,  // normal file
                       NULL);                  // no attr. template

    if (_hFile == INVALID_HANDLE_VALUE) 
    { 
        //DisplayError(TEXT("CreateFile"));
        _tprintf(TEXT("Terminal failure: Unable to open file \"%s\" for write.\n"), wname);
        return;
    }

  }

  ~NativeFile()
  {
	  this->!NativeFile();

  }

  bool WriteFile(System::String ^data)
  {
	  pin_ptr<const wchar_t> wdata = PtrToStringChars(data); 
	  DWORD dwBytesToWrite=data->Length;
	  /*char *buffer=new char[data->Length];

	  for (int x=0; x< data->Length; x++)
	  {

		  buffer[x]=data[x];
	  }*/

	  DWORD dwBytesWritten=0;
//	  bool bSuccess=false;

	  int iSuccess = ::WriteFile( 
                    _hFile,           // open file handle
                    wdata,      // start of data to write
                    dwBytesToWrite*2,  // number of bytes to write
                    &dwBytesWritten, // number of bytes that were written
                    NULL);            // no overlapped structure

	  bool bSuccess=iSuccess>=1;

    if (FALSE == bSuccess)
    {
        //DisplayError(TEXT("WriteFile"));
        printf("Terminal failure: Unable to write to file.\n");
    }
    else
    {
        if (dwBytesWritten != dwBytesToWrite*2)
        {
            // This is an error because a synchronous write that results in
            // success (WriteFile returns TRUE) should write all data as
            // requested. This would not necessarily be the case for
            // asynchronous writes.
            printf("Error: dwBytesWritten != dwBytesToWrite\n");
        }
        
    }

	return bSuccess;
  }


  bool WriteFile(array<System::Byte> ^buffer)
  {
	  pin_ptr<System::Byte> chars = &buffer[0];
	  
	  DWORD dwBytesToWrite=buffer->Length;
	  DWORD dwBytesWritten=0;
	  //bool bSuccess=false;

	  int iSuccess = ::WriteFile( 
                    _hFile,           // open file handle
                    chars,      // start of data to write
                    dwBytesToWrite,  // number of bytes to write
                    &dwBytesWritten, // number of bytes that were written
                    NULL);            // no overlapped structure

	  bool bSuccess= iSuccess>=1;

    if (FALSE == bSuccess)
    {
        //DisplayError(TEXT("WriteFile"));
        printf("Terminal failure: Unable to write to file.\n");
    }
    else
    {
        if (dwBytesWritten != dwBytesToWrite)
        {
            // This is an error because a synchronous write that results in
            // success (WriteFile returns TRUE) should write all data as
            // requested. This would not necessarily be the case for
            // asynchronous writes.
            printf("Error: dwBytesWritten != dwBytesToWrite\n");
        }
        
    }

	return bSuccess;
  }

  bool SetFilePointer(long distanceToMove, MoveMethod moveMethod)
  {
	  LARGE_INTEGER liDistanceToMove;  
	  liDistanceToMove.QuadPart=distanceToMove;
	  return SetFilePointerEx(_hFile, liDistanceToMove, NULL, (DWORD)moveMethod)>0; 


  }

  bool ReadFile(array<System::Char> ^buffer, long %bytesRead)
  {
	  pin_ptr<System::Char> chars = &buffer[0];

	  DWORD bytesReadIn=0;
	  DWORD size=buffer->Length;

	  //interior_ptr<char *> chars=&buffer;
	   bool retval=::ReadFile(_hFile, chars, size, &bytesReadIn, NULL)>0;

	   bytesRead=bytesReadIn;

	   return retval;


  }

  
private:
  !NativeFile()
  {
	  if (_hFile!=INVALID_HANDLE_VALUE)
	  {
		  CloseHandle(_hFile);
		  _hFile=INVALID_HANDLE_VALUE;
	  }
  }


};

}

