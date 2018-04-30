using OAuthHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitHubViewer
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();
            GitHubOAuthHelpers.OnAuthChanged += AuthenticationChanged;
            Refresh();
        }

        private void AuthenticationChanged(bool state)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Refresh();
            });
        }

        public async void Refresh()
        {

            if (GitHubOAuthHelpers.response != null)
            {
                //Navigate to the user page!
                await Navigation.PushAsync(new UserDisplayPage());
                
            }
        }

        private void GitHubSignInClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(GitHubOAuthHelpers.GetOAuth()));

        }
    }
}
