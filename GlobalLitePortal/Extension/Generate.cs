using GlobalLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GlobalLitePortal.Extension
{
    public static class Generate
    {
        private static StringBuilder newSentence = new StringBuilder();
        public static string ByteArrayToImage(byte[] bytesArr)
        {
            var base64 = Convert.ToBase64String(bytesArr);
            var img = String.Format("data:image/gif;base64,{0}", base64);
            return img;
        }



        public static decimal GetretiredAmountByRequisitionId(long ExpenseRequisitionId)
        {

            try
            {
                #region Current Session Check
                var user = System.Web.HttpContext.Current.User.Identity.Name;
                var userData = MvcApplication.GetUserData(user);
                if (userData == null || userData.UserId < 1)
                {
                    return decimal.Zero;
                }


                #endregion
                var searchObj = new RetirementSearchObj
                {
                    AdminUserId = userData.UserId,
                    RetirementId = 0,
                    StartDate = "",
                    StopDate = "",
                    VoucheNumber = "",
                    Status = -1000
                };
                var retVal = RetirementServices.LoadRetirements(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return decimal.Zero;
                }
                if (retVal.Retirements == null || !retVal.Retirements.Any())
                {
                    return decimal.Zero;
                }
                var retirement = retVal.Retirements.SingleOrDefault(x => x.ExpenseRequisitionId == ExpenseRequisitionId);
                if (retirement == null)
                {
                    return decimal.Zero;
                }
                return retirement.AmountRetired;
            }
            catch (Exception ex)
            {
                return decimal.Zero;
            }
        }

        public static string TitleLength(string title)
        { 
            string sentence = title;
            string[] words = sentence.Split(' ');
            if (words.Length < 6) { return title; } 
            int myLimit = 4;
           
            string line = "";
            foreach (string word in words)
            {
                if ((line + word).Length > myLimit)
                {
                    newSentence.AppendLine(Environment.NewLine);
                    line = "";
                }

                line += string.Format("{0} ", word);
            }

            if (line.Length > 0)
                newSentence.AppendLine(line);
             
            return newSentence.ToString();
        }

    }


}