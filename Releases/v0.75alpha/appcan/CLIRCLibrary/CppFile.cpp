// CppFile.cpp : main project file.

#include "stdafx.h"
#include <fstream>
#include <windows.h>
#include <tchar.h>
#include <stdio.h>
#include <strsafe.h>



using namespace System;

int WriteToFileTest()
{
	HANDLE hFile; 
    char DataBuffer[] = "This is some test data to write to the file.";
    DWORD dwBytesToWrite = (DWORD)strlen(DataBuffer);
    DWORD dwBytesWritten = 0;
    BOOL bErrorFlag = FALSE;

    printf("\n");
	TCHAR fileName[]=_T("test.txt");

    

    hFile = CreateFile(fileName,                // name of the write
                       GENERIC_WRITE,          // open for writing
                       0,                      // do not share
                       NULL,                   // default security
                       CREATE_NEW,             // create new file only
                       FILE_ATTRIBUTE_NORMAL,  // normal file
                       NULL);                  // no attr. template

    if (hFile == INVALID_HANDLE_VALUE) 
    { 
        //DisplayError(TEXT("CreateFile"));
        _tprintf(TEXT("Terminal failure: Unable to open file \"%s\" for write.\n"), fileName);
        return 0;
    }

    _tprintf(TEXT("Writing %d bytes to %s.\n"), dwBytesToWrite, fileName);

    bErrorFlag = WriteFile( 
                    hFile,           // open file handle
                    DataBuffer,      // start of data to write
                    dwBytesToWrite,  // number of bytes to write
                    &dwBytesWritten, // number of bytes that were written
                    NULL);            // no overlapped structure

    if (FALSE == bErrorFlag)
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
        else
        {
            _tprintf(TEXT("Wrote %d bytes to %s successfully.\n"), dwBytesWritten, fileName);
        }
    }

    CloseHandle(hFile);

	return 1;

}


int main(array<System::String ^> ^args)
{
    Console::WriteLine(L"Hello World");
	std::ofstream of("test.txt");
	of << "This is a test content.";
	of.close();

    return 0;
}
