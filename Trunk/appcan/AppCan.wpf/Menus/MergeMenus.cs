/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections;
using System.ComponentModel;


namespace AppCan.wpf.Menus
{
    /// <summary>
    /// Utility class for WPF MergeMenu support
    /// This file/class is subject to the licensing indicated in the CPOL.htm file
    /// Items in the host menu are expected to have their priorities already set in sorted order, from lowest to highest.
    /// Merged items can be added in any order since they will be added to the host menu one at a time.
    /// Items with the same priority, belong to the same group that will get seperators.  Main menu (top level items) don't get seperators
    /// If IsMenuMenu is set to true it will not get seperators.  Once added items can not be removed.  If you do not want them displayed then
    /// change the Visibility of the menu item.  Menu's and Toolbars must be created to be merged.   If added to window resources on load window should call
    /// FindResource on the resource to create them.  One menu is visible and acts as the host.  Other menus that will merge into it appear in 
    /// resources and specify a HostId of where it will merge into.
    /// Code is a modified version from
    /// http://www.codeproject.com/Articles/112895/Automatic-Merging-of-Menus-and-Toolbars-in-WPF
    /// </summary>
    public static class MergeMenus
    {
        /// <summary>
        /// Attached dependency property to add a id to WPF Tollbars or Menus.
        /// Menu or Toolbar (or ToolBarTray) is a valid merge host then.
        /// </summary>
        /// <remarks>
        /// Object must be derived from ItemsControl or ToolBarTray to attach this property!
        /// </remarks>
        public static readonly DependencyProperty IdProperty = DependencyProperty.RegisterAttached("Id",
           typeof(string), typeof(MergeMenus), new FrameworkPropertyMetadata(null, OnIdChanged));

        /// <summary>
        /// Sets the id
        /// </summary>
        /// <param name="d">Object to set the id for</param>
        /// <param name="value">New id</param>
        public static void SetId(DependencyObject d, string value)
        {
            d.SetValue(IdProperty, value);
        }

        /// <summary>
        /// Gets the id
        /// </summary>
        /// <param name="d">Object to get the menu id from</param>
        /// <returns>Returns the id of the given object.</returns>
        public static string GetId(DependencyObject d)
        {
            return (string)d.GetValue(IdProperty);
        }

        /// <summary>
        /// Is called when the id of an object is changed.
        /// </summary>
        /// <param name="d">Object</param>
        /// <param name="e">Event args</param>
        /// <remarks>
        /// Adds the object to a dictionary with all hosts and registers an Initialized event handler.
        /// </remarks>
        private static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // check if object is an ItemsControl or ToolBarTray (is a must for menu hosts)
            if (!(d is ItemsControl) && !(d is ToolBarTray))
            {
                throw new ArgumentException("Attached property \'Id\' con only be applied to ItemsControls or ToolBarTrays");
            }

            var oldId = (string)e.OldValue;
            var newId = (string)e.NewValue;

            // unregister with old id (if possible) 
            if (!String.IsNullOrWhiteSpace(oldId) && _MergeHosts.ContainsKey(oldId))
            {
                MergeHost host;
                if (_MergeHosts.TryGetValue(oldId, out host))
                {
                    host.HostElement = null;
                    _MergeHosts.Remove(oldId);
                }
            }
            // register with new id
            if (!String.IsNullOrWhiteSpace(newId))
            {
                var host = new MergeHost(newId);
                host.HostElement = d as FrameworkElement;
                _MergeHosts.Add(newId, host);
            }
        }


        /// <summary>
        /// Attached dependency property to apply a priority to menu or tool bar items
        /// </summary>
        public static readonly DependencyProperty PriorityProperty = DependencyProperty.RegisterAttached("Priority",
           typeof(int), typeof(MergeMenus), new FrameworkPropertyMetadata(0));

        /// <summary>
        /// Sets the priority for merge items
        /// </summary>
        /// <param name="d">Item</param>
        /// <param name="value">Priority</param>
        public static void SetPriority(DependencyObject d, int value)
        {
            d.SetValue(PriorityProperty, value);
        }

        /// <summary>
        /// Gets the priority from an merge item
        /// </summary>
        /// <param name="d">Item</param>
        /// <returns>Returns the priority from an merge item.</returns>
        /// <remarks>
        /// If no priority is attached then 0 is returned
        /// </remarks>
        public static int GetPriority(DependencyObject d)
        {
            return (int)d.GetValue(PriorityProperty);
        }

        /// <summary>
        /// Gets the priority from an merge item
        /// </summary>
        /// <param name="d">Item</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Returns the priority from an merge item.</returns>
        /// <remarks>
        /// If no priority is attached then defaultValue is returned
        /// </remarks>
        public static int GetPriorityDef(DependencyObject d, int defaultValue)
        {
            var oPriority = d.GetValue(PriorityProperty);
            if (oPriority == null)
            {
                return defaultValue;
            }
            else
            {
                return (int)oPriority;
            }
        }


        /// <summary>
        /// Attached dependency property to add the AddSeparatorBehaviour
        /// </summary>
        public static readonly DependencyProperty AddSeparatorProperty = DependencyProperty.RegisterAttached("AddSeparator",
           typeof(AddSeparatorBehaviour), typeof(MergeMenus), new FrameworkPropertyMetadata(AddSeparatorBehaviour.Default));

        /// <summary>
        /// Sets theAddSeparatorBehaviour for this item
        /// </summary>
        /// <param name="d">Item</param>
        /// <param name="value">AddSeparatorBehaviour</param>
        public static void SetAddSeparator(DependencyObject d, AddSeparatorBehaviour value)
        {
            d.SetValue(AddSeparatorProperty, value);
        }

        /// <summary>
        /// Gets the AddSeparatorBehaviour of this item.
        /// </summary>
        /// <param name="d">Item</param>
        /// <returns>Returns the AddSeparatorBehaviour of this item.</returns>
        public static AddSeparatorBehaviour GetAddSeparator(DependencyObject d)
        {
            return (AddSeparatorBehaviour)d.GetValue(AddSeparatorProperty);
        }


        /// <summary>
        /// Attached dependency property to add the id of the host into which this item should be merged
        /// </summary>
        public static readonly DependencyProperty HostIdProperty = DependencyProperty.RegisterAttached("HostId",
           typeof(string), typeof(MergeMenus), new FrameworkPropertyMetadata(null, OnHostIdChanged));

        /// <summary>
        /// Sets the merge host for this item
        /// </summary>
        /// <param name="d">Item</param>
        /// <param name="value">Host id</param>
        public static void SetHostId(DependencyObject d, string value)
        {
            d.SetValue(HostIdProperty, value);
        }

        /// <summary>
        /// Gets the merge host id of this item.
        /// </summary>
        /// <param name="d">Item</param>
        /// <returns>Returns the merge host id of this item.</returns>
        public static string GetHostId(DependencyObject d)
        {
            return (string)d.GetValue(HostIdProperty);
        }

        /// <summary>
        /// Is called when the merge host id changes for an item
        /// </summary>
        /// <param name="d">Item</param>
        /// <param name="e">Event args</param>
        /// <remarks>
        /// Adds the item to a list and adds a Initialized event handler
        /// </remarks>
        private static void OnHostIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldHostId = (string)e.OldValue;
            var newHostId = (string)e.NewValue;

            // unregister item
            if (!String.IsNullOrWhiteSpace(oldHostId) && _UnmergedItems.Contains(d))
            {
                if (d is FrameworkElement)
                {
                    (d as FrameworkElement).Initialized -= UnmergedItem_Initialized;
                }

                _UnmergedItems.Remove(d);
            }

            // register item
            if (!String.IsNullOrWhiteSpace(newHostId))
            {
                _UnmergedItems.Add(d);

                if (d is FrameworkElement)
                {
                    (d as FrameworkElement).Initialized += UnmergedItem_Initialized;
                }
            }
        }

        /// <summary>
        /// Initialized event handler for merge items
        /// </summary>
        /// <param name="sender">Item</param>
        /// <param name="e">Event args</param>
        /// <remarks>
        /// Adds this item to a host if not already happend.
        /// </remarks>
        private static void UnmergedItem_Initialized(object sender, EventArgs e)
        {
            var item = sender as DependencyObject;
            var hostId = GetHostId(item);
            MergeHost host;
            if (_MergeHosts.TryGetValue(hostId, out host))
            {
                if (host.MergeItem(item))
                {
                    _UnmergedItems.Remove(item);
                }
            }
        }


        /// <summary>
        /// Dictionary with all hosts
        /// </summary>
        /// <remarks>
        /// Id maps to host.
        /// </remarks>
        private static Dictionary<string, MergeHost> _MergeHosts = new Dictionary<string, MergeHost>();

        /// <summary>
        /// Gets the dictionary with all hosts.
        /// </summary>
        /// <remarks>
        /// Id maps to host.
        /// </remarks>
        public static IDictionary<string, MergeHost> MergeHosts
        {
            get { return _MergeHosts; }
        }


        /// <summary>
        /// List with all so far unmerged items
        /// </summary>
        private static List<DependencyObject> _UnmergedItems = new List<DependencyObject>();

        /// <summary>
        /// Gets the list with all so far unmerged items
        /// </summary>
        public static IList<DependencyObject> UnmergedItems
        {
            get { return _UnmergedItems; }
        }
    }

    /// <summary>
    /// Class representing a merge host (ItemsControl or ToolBarTray)
    /// </summary>
    public class MergeHost
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of the host</param>
        internal MergeHost(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the id of the host.
        /// </summary>
        public string Id { get; private set; }

        private FrameworkElement _HostElement = null;

        /// <summary>
        /// Gets or sets the host element
        /// </summary>
        public FrameworkElement HostElement
        {
            get { return _HostElement; }
            internal set
            {
                if (_HostElement != null)
                {
                    _HostElement.Initialized -= HostElement_Initialized;
                }

                _HostElement = value;

                if (_HostElement != null)
                {
                    _HostElement.Initialized += HostElement_Initialized;
                }
            }
        }

        /// <summary>
        /// Initialized event handler for hosts
        /// </summary>
        /// <param name="sender">Host</param>
        /// <param name="e">Event args</param>
        /// <remarks>
        /// Adds any known merge items to the host if not already happend.
        /// </remarks>
        private void HostElement_Initialized(object sender, EventArgs e)
        {
            if (HostElement != null)
            {
                var id = MergeMenus.GetId(sender as DependencyObject);
                foreach (var item in MergeMenus.UnmergedItems.ToList())
                {
                    if (String.CompareOrdinal(id, MergeMenus.GetHostId(item)) == 0)
                    {
                        if (MergeItem(item))
                        {
                            MergeMenus.UnmergedItems.Remove(item);
                        }
                    }
                }
            }
        }

        private List<DependencyObject> _MergedItems = new List<DependencyObject>();

        private List<Separator> _AutoCreatedSeparators = new List<Separator>();

        /// <summary>
        /// Merges the item into this host
        /// </summary>
        /// <param name="item">Item</param>
        internal bool MergeItem(DependencyObject item)
        {
            bool itemAdded = false;

            // get the priority of the item (if non is attached use highest priority)
            int priority = MergeMenus.GetPriorityDef(item, Int32.MaxValue);

            if (HostElement != null)
            {
                if (HostElement is ToolBarTray)
                {
                    /// special traetment for ToolBarTray hosts becuse a ToolBarTray is no ItemsControl.
                    if (item is ToolBar && !(HostElement as ToolBarTray).ToolBars.Contains(item))
                    {
                        (HostElement as ToolBarTray).ToolBars.Add(item as ToolBar);
                    }
                    itemAdded = true;
                }
                else
                {
                    var items = (HostElement as ItemsControl).Items;
                    // if item is not already in host add it by priority
                    if (!items.Contains(item))
                    {
                        // iterate from behind...
                        for (int n = items.Count - 1; n >= 0; --n)
                        {
                            var d = items[n] as DependencyObject;
                            if (d != null)
                            {
                                // ... and add it after 1st existing item with lower or equal priority
                                if (MergeMenus.GetPriority(d) <= priority)
                                {
                                    ++n;
                                    itemAdded = true;
                                    items.Insert(n, item);

                                    // add separators where necessary, but not on a main menu
                                    if (ShouldAddSeperators())
                                    {
                                        // if before us is a non separator and it's priority is different to ours, then insert a separator
                                        if (n > 0 && !(items[n - 1] is Separator))
                                        {
                                            int prioBefore = MergeMenus.GetPriority(items[n - 1] as DependencyObject);
                                            if (priority != prioBefore)
                                            {
                                                var separator = new Separator();
                                                MergeMenus.SetPriority(separator, priority);
                                                items.Insert(n, separator);
                                                _AutoCreatedSeparators.Add(separator);
                                                ++n;
                                            }
                                        }

                                        // if after us is a non seperator then add a separator after us
                                        if (n < items.Count - 1 && !(items[n + 1] is Separator))
                                        {
                                            int prioAfter = MergeMenus.GetPriority(items[n + 1] as DependencyObject);
                                            var separator = new Separator();
                                            MergeMenus.SetPriority(separator, prioAfter);
                                            items.Insert(n + 1, separator);
                                            _AutoCreatedSeparators.Add(separator);
                                        }
                                    }
                                    break;
                                }
                            }
                        }

                        if (!itemAdded)
                        {
                            // if item is not added for any reason so far, simply add it to beginning since no other space was found
                            items.Insert(0,item);
                        }
                        _MergedItems.Add(item);

                        // register a VisibilityChanged notifier to hide seperators if necessary
                        if (item is UIElement)
                        {
                            DependencyPropertyDescriptor.FromProperty(UIElement.VisibilityProperty, item.GetType()).AddValueChanged(item, Item_VisibilityChanged);
                        }
                        CheckSeparatorVisibility(true);
                    }
                    else
                    {
                        itemAdded = true;
                    }
                }
            }
            return itemAdded;
        }

        /// <summary>
        /// Checks if seperators should be added for this host
        /// </summary>
        /// <returns></returns>
        private bool ShouldAddSeperators()
        {
            switch (MergeMenus.GetAddSeparator(HostElement))
            {
                case AddSeparatorBehaviour.Add:
                    return true;

                case AddSeparatorBehaviour.DontAdd:
                    return false;

                default:
                    // default is add, except for ToolBarTrays and MainMenus
                    return (!(HostElement is ToolBarTray)) && (!(HostElement is Menu) || !(HostElement as Menu).IsMainMenu);
            }
        }

        /// <summary>
        /// Callback whenn the Visibility of an item changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_VisibilityChanged(object sender, EventArgs e)
        {
            CheckSeparatorVisibility((sender as UIElement).Visibility != Visibility.Visible);
        }

        /// <summary>
        /// Hides or shows automatically inserted Separators if necessary.
        /// </summary>
        /// <param name="itemWasHidden"></param>
        private void CheckSeparatorVisibility(bool itemWasHidden)
        {
            if (HostElement != null && HostElement is ItemsControl)
            {
                var items = (HostElement as ItemsControl).Items;

                // check if we need to hide any separators
                if (itemWasHidden)
                {
                    foreach (var separator in _AutoCreatedSeparators)
                    {
                        if (separator.Visibility == Visibility.Visible)
                        {
                            int idx = items.IndexOf(separator);

                            int n = idx - 1;
                            while (n >= 0)
                            {
                                var uie = items[n] as UIElement;
                                if (uie != null)
                                {
                                    if ((uie is Separator) && uie.Visibility == Visibility.Visible)
                                    {
                                        separator.Visibility = Visibility.Collapsed;
                                        break;
                                    }
                                    else if (uie.Visibility == Visibility.Visible)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                                --n;
                            }
                            if (n < 0)
                            {
                                separator.Visibility = Visibility.Collapsed;
                            }
                        }
                        if (separator.Visibility == Visibility.Visible)
                        {
                            int idx = items.IndexOf(separator);

                            int n = idx + 1;
                            while (n < items.Count)
                            {
                                var uie = items[n] as UIElement;
                                if (uie != null)
                                {
                                    if ((uie is Separator) && uie.Visibility == Visibility.Visible)
                                    {
                                        separator.Visibility = Visibility.Collapsed;
                                        break;
                                    }
                                    else if (uie.Visibility == Visibility.Visible)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                                ++n;
                            }
                            if (n >= items.Count)
                            {
                                separator.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
                else
                {
                    // check if we need to make any separator visible
                    foreach (var separator in _AutoCreatedSeparators)
                    {
                        if (separator.Visibility != Visibility.Visible)
                        {
                            bool shouldBeHidden = false;
                            int idx = items.IndexOf(separator);

                            int n = idx - 1;
                            while (n >= 0)
                            {
                                var uie = items[n] as UIElement;
                                if (uie != null)
                                {
                                    if ((uie is Separator) && uie.Visibility == Visibility.Visible)
                                    {
                                        shouldBeHidden = true;
                                        break;
                                    }
                                    else if (uie.Visibility == Visibility.Visible)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                                --n;
                            }

                            if (!shouldBeHidden)
                            {
                                n = idx + 1;
                                while (n < items.Count)
                                {
                                    var uie = items[n] as UIElement;
                                    if (uie != null)
                                    {
                                        if ((uie is Separator) && uie.Visibility == Visibility.Visible)
                                        {
                                            shouldBeHidden = true;
                                            break;
                                        }
                                        else if (uie.Visibility == Visibility.Visible)
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    ++n;
                                }
                            }

                            if (!shouldBeHidden)
                            {
                                separator.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Behaviour for atomatically adding seperators to hosts
    /// </summary>
    public enum AddSeparatorBehaviour
    {
        /// <summary>
        /// Use default behaviour dependent on host type
        /// </summary>
        Default,

        /// <summary>
        /// Always add separators
        /// </summary>
        Add,

        /// <summary>
        /// Don't add separators
        /// </summary>
        DontAdd
    }
}
