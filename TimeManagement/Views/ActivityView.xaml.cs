using System;
using TimeManagement.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityView : ContentPage, IHasCollectionView
    {
        public ActivityView()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AnalyticsHelper.TrackEventAsync("ActivityView opened");
        }

        public CollectionView CollectionView => CollectionView1;
        protected override void OnBindingContextChanged()
        {
            if (this.BindingContext is IHasCollectionViewModel hasCollectionViewModel)
            {
                hasCollectionViewModel.View = this;
            }
            base.OnBindingContextChanged();
        }
    }
}