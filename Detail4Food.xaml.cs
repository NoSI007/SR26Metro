using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using W8rtmGrid.Common;
using W8rtmGrid.Data;
using W8rtmGrid.Data.Sr26d;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace W8rtmGrid
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class Detail4Food : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        SampleDataItem SelectedItem = null;
        /// <summary>
        /// This can be changed to a strongly typed view model. 
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public Detail4Food()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private  void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            //item = await SampleDataSource.GetItemAsync((String)e.NavigationParameter);
            //this.DefaultViewModel["Item"] = item;
            //ItemClickEventArgs itm = (ItemClickEventArgs)e.NavigationParameter;
            //string iid = ((SampleDataItem)itm.ClickedItem).UniqueId;
            //if (string.IsNullOrWhiteSpace(iid) == false)
            //{
            //    SelectedItem = await SampleDataSource.GetItemAsync(iid);
            //    this.DefaultViewModel["SelectedItem"] = SelectedItem;
            //}


            //object navigationParameter;
            //if (e.PageState != null && e.PageState.ContainsKey("SelectedItem"))
            //{
            //    navigationParameter = e.PageState["SelectedItem"];
            //}
            Register4HtmlShare();
            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
            // TODO: Assign the selected item to this.flipView.SelectedItem

        }
        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            UnRegister4HtmlShare();
        }
        #region NavigationHelper registration
        /*
          itemTitle.Text = string.Format("{0}", si.Description);
                    itemDetailTitlePanel.DataContext = si;
                    NutsList4Foo.DataContext = Sr26Data.NutDetail;

                    ItemsCount.Text = string.Format("{0} Items found (NDB_No={1})", Sr26Data.NutDetail.Count, ndbno);

       */
        private void getPageData()
        {

            try
            {
                if (SelectedItem != null)
                {
                    pageTitle.Text = string.Format("Nutrients for {0}", SelectedItem.Description);
                    WvDetail.NavigateToString(Sr26Data.Text2Html(SelectedItem.Content));
                    //ItemsCount.Text = string.Format("{0} Items found (NDB_No={1})", Sr26Data.NutDetail.Count, ndbno);
                }
            }
            catch
            {

            }
        }






        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string iid = (string)e.Parameter;

            SelectedItem = await SampleDataSource.GetItemAsync(iid);
   
            getPageData();

            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        #region share
        private void Register4HtmlShare()// call this in last line of the navigationHelper_LoadState 
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareHtmlHandler);
        }
        private void UnRegister4HtmlShare()// call this in the last line of navigationHelper_SaveState 
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.ShareHtmlHandler);
        }
        private void ShareHtmlHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            /* see link for Html share.
             http://msdn.microsoft.com/en-us/library/windows/apps/hh973055.aspx
             */

            //var si = itemListView.SelectedItem;

            try
            {
                DataRequest request = e.Request;

                if (SelectedItem != null)
                {
                    request.Data.Properties.Title = SelectedItem.Description;
                    request.Data.Properties.Description = "The Requested Food";
                    string  nutrients = Sr26Data.Text2Html(SelectedItem.Content);
                    string htmlFormat = HtmlFormatHelper.CreateHtmlFormat(nutrients);
                    request.Data.SetHtmlFormat(htmlFormat);
                }
            }
            catch
            {

            }


        }


        #endregion share


        private void ShowUIButton_Click(object sender, RoutedEventArgs e)
        {
            //HACK:when the data is not present the application exits.
            DataTransferManager.ShowShareUI();
        }

        private void zPin_Click(object sender, RoutedEventArgs e)
        {
            pintile();
        }

        private async void pintile()
        {
            if (SelectedItem == null)
                return;
            //Uri square70x70Logo = new Uri("ms-appx:///Assets/square70x70Tile-sr26.png");
            Uri square150x150Logo = new Uri("ms-appx:///Assets/Logo.scale-100.png");//
            Uri wide310x150Logo = new Uri("ms-appx:///Assets/wide.scale-100.png");
            //Uri square310x310Logo = new Uri("ms-appx:///Assets/square310x310Tile-sr26.png");
            Uri square30x30Logo = new Uri("ms-appx:///Assets/SmallLogo.scale-100.png", UriKind.Absolute);


            SecondaryTile secondaryTile = new SecondaryTile();//
            secondaryTile.Arguments = SelectedItem.UniqueId;//SelectedItem
            secondaryTile.TileId = SelectedItem.UniqueId;
            secondaryTile.DisplayName = SelectedItem.Description;


            secondaryTile.DisplayName = SelectedItem.Subtitle;
            secondaryTile.TileOptions = TileOptions.ShowNameOnLogo;
            secondaryTile.Logo = square150x150Logo;
            secondaryTile.SmallLogo = square30x30Logo;

            secondaryTile.VisualElements.Wide310x150Logo = wide310x150Logo;
            //secondaryTile.VisualElements.Square310x310Logo = square310x310Logo;
            //secondaryTile.VisualElements.Square70x70Logo = square70x70Logo;
            secondaryTile.VisualElements.Square30x30Logo = square30x30Logo;

            secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
            secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;

            // Specify a foreground text value.
            // The tile background color is inherited from the parent unless a separate value is specified.
            secondaryTile.VisualElements.ForegroundText = ForegroundText.Light;

            secondaryTile.RoamingEnabled = true;

            // OK, the tile is created and we can now attempt to pin the tile.
            // Note that the status message is updated when the async operation to pin the tile completes.
            //bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GroupDetailPage.GetElementRect((FrameworkElement)sender), Windows.UI.Popups.Placement.Below);

            try
            {
                bool isPinned = await secondaryTile.RequestCreateAsync();
                //pageTitle.Text = string.Format(" result of pinning is = {0}", isPinned);
            }
            catch (System.Exception e)
            {
                //pageTitle.Text = e.Message;
            }
        }

        private void zFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchPane spc = SearchPane.GetForCurrentView();
                spc.Show("");
            }
            catch { }
        }
    }
}
