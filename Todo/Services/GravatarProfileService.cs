using GravatarSharp.Core;
using GravatarSharp.Core.Model;
using System;
using System.Collections.Generic;

namespace Todo.Services
{
    public class GravatarProfileService
    {
        private GravatarController gravatarController;
        private Dictionary<string, GravatarProfile> gravatarProfileCache;

        private static GravatarProfileService instance;

        private GravatarProfileService()
        {
            gravatarController = new GravatarController();
            gravatarProfileCache = new Dictionary<string, GravatarProfile>();
        }

        /// <summary>
        /// Gets the singleton Gravatar Profile service for static use.
        /// </summary>
        /// <returns>Singleton Gravatar Profile service</returns>
        public static GravatarProfileService GetService()
        {
            if (instance == null)
            {
                instance = new GravatarProfileService();
            }

            return instance;

        }

        public GravatarProfile GetGravatarProfile(string emailAddress)
        {
            if (emailAddress == null)
            {
                return null;
            }

            // Try to get the cached value;
            if (gravatarProfileCache.TryGetValue(emailAddress, out var cachedProfile))
            {
                return cachedProfile;
            }

            // TODO: Change this to use async tasks all the way through the call stack.
            try
            {
                var gravatarProfileResult = gravatarController.GetProfile(emailAddress).Result;

                if (gravatarProfileResult.Profile != null && gravatarProfileResult.ErrorMessage == null)
                {
                    gravatarProfileCache.Add(emailAddress, gravatarProfileResult.Profile);
                    return gravatarProfileResult.Profile;
                }
            }
            catch (Exception e)
            {
                // TODO: Handle Gravatar profile request failiures
            }

            return null;
        }
    }
}
