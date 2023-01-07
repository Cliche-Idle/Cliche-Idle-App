using System;
using System.Collections.Generic;
using Cliche.Idle;

namespace Assets.Lib.Data.API.Cliche.Manifests
{
    public static partial class GameStringBuilder
    {
        /// <summary>
        /// The list of valid gameString tokens and their replacers.
        /// Every replacer function must return a <see cref="string"/>, 
        /// and take three params in this order: Token, RawString, SourceObject.
        /// </summary>
        private static readonly Dictionary<string, Func<string, string, object, string>> _gameStringTokens = new Dictionary<string, Func<string, string, object, string>>()
        {
            { "#player_name", InsertPlayerName },

            { "#item_name", InsertItemName },
            { "#main_stat_value", InsertItemMainStatValue },
            { "#main_stat_name", InsertItemMainStatName },

            { "#req_level", InsertRequirementLevel },
            { "#req_race", InsertRequirementRace },
        };

        /// <summary>
        /// Replaces every token in a string, based on the given <paramref name="source"/> object. Tokens not compatible with <paramref name="source"/> will be skipped.
        /// </summary>
        /// <param name="rawString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string BuildGameString(string rawString, object source)
        {
            // Replaces complex value tokens
            foreach (var token in _gameStringTokens)
            {
                if (rawString.Contains(token.Key))
                {
                    rawString = token.Value.Invoke(token.Key, rawString, source);
                }
            }
            return rawString;
        }

        /// <summary>
        /// Inserts the Player character's name into the game string.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="rawString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string InsertPlayerName(string token, string rawString, object source)
        {
            rawString.Replace(token, Player.Character.CharacterSheet.Name);
            return rawString;
        }

        /// <summary>
        /// Inserts the given <see cref="Item"/>'s <see cref="ItemManifest.Name"/> into the game string.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="rawString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string InsertItemName(string token, string rawString, object source)
        {
            var item = source as Item;
            if (item != null)
            {
                rawString.Replace(token, $"{item.GetManifest().Name}");
            }
            return rawString;
        }

        /// <summary>
        /// Inserts the given <see cref="Item"/>'s <see cref="Item.MainStatValue"/> into the game string.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="rawString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string InsertItemMainStatValue(string token, string rawString, object source)
        {
            var item = source as Item;
            if (item != null)
            {
                rawString.Replace(token, $"{item.MainStatValue}");
            }
            return rawString;
        }

        /// <summary>
        /// Inserts the given <see cref="Item"/>'s <see cref="Item.MainStatType"/> into the game string.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="rawString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string InsertItemMainStatName(string token, string rawString, object source)
        {
            var item = source as Item;
            if (item != null)
            {
                rawString.Replace(token, $"{item.MainStatType}");
            }
            return rawString;
        }

        /// <summary>
        /// Inserts the given <see cref="AdventureManifest"/>'s <see cref="Requirements.Level"/> into the game string.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="rawString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string InsertRequirementLevel(string token, string rawString, object source)
        {
            var adventure = source as AdventureManifest;
            if (adventure != null)
            {
                if (adventure.Requirements != null)
                {
                    rawString.Replace(token, $"{adventure.Requirements.Level}");
                }
            }
            return rawString;
        }

        /// <summary>
        /// Inserts the given <see cref="AdventureManifest"/>'s <see cref="Requirements.Race"/> into the game string.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="rawString"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static string InsertRequirementRace(string token, string rawString, object source)
        {
            var adventure = source as AdventureManifest;
            if (adventure != null)
            {
                if (adventure.Requirements != null)
                {
                    rawString.Replace(token, $"{adventure.Requirements.Race}");
                }
            }
            return rawString;
        }






        /// <summary>
        /// Gets a raw gameString with the given ID.
        /// </summary>
        /// <param name="localeStringID"></param>
        /// <returns><see cref="null"/></returns>
        [Obsolete]
        private static string GetRawLocaleStringByCode(string localeStringID)
        {
            string localeRawString = null;
            return localeRawString;
        }
    }
}
