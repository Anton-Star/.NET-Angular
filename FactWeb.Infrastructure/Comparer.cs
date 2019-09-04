using FactWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactWeb.Infrastructure
{
    public class Comparer
    {
        public static bool DoesNotMatch(DateTime? original, DateTime? newItem)
        {
            return (original == null && newItem != null) || (original != null && newItem == null) || original != newItem;
        }

        public static bool DoesNotMatch(int? original, int? newItem)
        {
            return (original == null && newItem != null) || (original != null && newItem == null) || original != newItem;
        }

        public static string OrderSite(Site site, List<int> strongRelations = null)
        {
            var sort = "";

            if (site == null) return "J";

            sort = site.IsPrimarySite ? "1" : "2";


            if (strongRelations != null && strongRelations.Any(x => x == site.Id))
            {
                sort += "1";
            }
            else
            {
                sort += "2";
            }

            var facSite = site.FacilitySites?.FirstOrDefault();

            if (facSite == null)
            {
                sort += "J";
            }
            else
            {
                switch (facSite.Facility.ServiceTypeId)
                {
                    case (int)Constants.ServiceTypes.CbBank:
                        sort += "A";
                        break;
                    case (int)Constants.ServiceTypes.CbCollection:
                        sort += "B";
                        break;
                    case (int)Constants.ServiceTypes.CbProcessing:
                        sort += "C";
                        break;
                    case (int)Constants.ServiceTypes.CbStorage:
                        sort += "D";
                        break;
                    case (int)Constants.ServiceTypes.ClinicalProgramCt:
                        sort += "E";
                        break;
                    case (int)Constants.ServiceTypes.MarrowCollectionCt:
                        sort += "F";
                        break;
                    case (int)Constants.ServiceTypes.ApheresisCollection:
                        sort += "G";
                        break;
                    case (int)Constants.ServiceTypes.ProcessingCt:
                        sort += "H";
                        break;
                    default:
                        sort += "I";
                        break;
                }
            }

            if (site.SiteTransplantTypes == null || site.SiteTransplantTypes.Count == 0)
            {
                sort += "E";
            }
            else
            {
                foreach (var tt in site.SiteTransplantTypes)
                {
                    if (tt.TransplantTypeId == (int)Constants.TransplantTypes.AllogeneicAdult)
                    {
                        sort += "A";
                        break;
                    }

                    if (tt.TransplantTypeId == (int)Constants.TransplantTypes.AutologousAdult)
                    {
                        sort += "B";
                        break;
                    }

                    if (tt.TransplantTypeId == (int)Constants.TransplantTypes.AllogeneicPediatric)
                    {
                        sort += "C";
                        break;
                    }

                    if (tt.TransplantTypeId == (int)Constants.TransplantTypes.AutologousPediatric)
                    {
                        sort += "D";
                        break;
                    }
                }
            }

            return sort;

        }
    }
}
