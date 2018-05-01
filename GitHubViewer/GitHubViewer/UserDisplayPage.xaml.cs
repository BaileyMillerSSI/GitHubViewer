using OAuthHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitHubViewer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDisplayPage : ContentPage
    {
        static RepoGroup RepoView;
        private string htmlLink;

        public UserDisplayPage()
        {
            InitializeComponent();
            GroupedView.ItemsSource = RepoGroup.All;


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Never be able to navigate back to myself
            var list = Navigation.NavigationStack;

            if (list.Count == 3 && list.Last() == this)
            {
                Navigation.RemovePage(Navigation.NavigationStack.ElementAt(2));
            }
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var avImage = this.FindByName<Image>("AvatarImage");
            
            var user = await GitHubLoaders.GetUser();

            if (user != null)
            {
                var userEmailLabel = this.FindByName<Label>("UserEmail");
                userEmailLabel.Text = user.name;
                htmlLink = user.html_url;

                avImage.Source = new UriImageSource
                {
                    Uri = new Uri(user.avatar_url),
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(1, 0, 0, 0)
                };
            }

            var pubRepo = await GitHubLoaders.GetPublicRepos();

            if (pubRepo != null)
            {
                var group = new RepoGroup("Public", "Public");

                foreach (var repo in pubRepo)
                {
                    group.Add(new RepoData(repo.name, String.IsNullOrEmpty(repo.language) ? "No Coding Language": repo.language, repo.html_url));
                }

                RepoGroup.AddGroup(group);
            }

            var privRepo = await GitHubLoaders.GetPublicRepos();

            if (privRepo != null)
            {
                var group = new RepoGroup("Private", "Private");

                foreach (var repo in privRepo)
                {
                    group.Add(new RepoData(repo.name, String.IsNullOrEmpty(repo.language) ? "No Coding Language" : repo.language, repo.html_url));
                }

                RepoGroup.AddGroup(group);
            }
        }

        private void GroupedView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Deselect Item
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem == null)
                return;

            var data = e.SelectedItem as RepoData;
            Device.OpenUri(new Uri(data.Url));
        }

        private void HtmlLabelTapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(htmlLink));
        }
    }
    
}
