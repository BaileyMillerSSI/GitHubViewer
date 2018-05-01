using OAuthHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GitHubViewer
{
    public class RepoGroup : List<RepoData>
    {
        public string Title { get; set; }
        public string ShortName { get; set; } //will be used for jump lists
        public string Subtitle { get; set; }
        public RepoGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
        }

        

        public static ObservableCollection<RepoGroup> All { private set; get; }

        public static void AddGroup(RepoGroup group)
        {
            if (All.Where(x=>x.Title == group.Title).Count() == 0)
            {
                All.Add(group);
            }
        }

        static RepoGroup()
        {
            ObservableCollection<RepoGroup> Groups = new ObservableCollection<RepoGroup>();

            All = Groups; //set the publicly accessible list
        }

    }

    public class RepoData
    {

        public String Name { private set; get; }
        public String Language { private set; get; }
        public String Url { private set; get; }

        public RepoData(string name, string language, string url)
        {
            Name = name;
            Language = language;
            Url = url;
        }
    }

}
