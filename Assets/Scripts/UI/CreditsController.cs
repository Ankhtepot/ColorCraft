using UnityEngine;
using Utilities;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class CreditsController : MonoBehaviour
    {
#pragma warning disable 649
    
#pragma warning restore 649

        /// <summary>
        /// Run from HomepageLinkButton
        /// </summary>
        public void OpenHomepage()
        {
            Application.OpenURL(Strings.HomePageLink);
        }
        
        /// <summary>
        /// Run from BlogLinkButton
        /// </summary>
        public void OpenBlogPost()
        {
            Application.OpenURL(Strings.ProjectLink);
        }
    }
}
