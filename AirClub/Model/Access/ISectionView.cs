using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Access
{
    public interface ISectionView
    {
        IAccessSection AccessSection { get; set; }
        IUserAccessOnSection UserSection { get; set; }

        public string Name { get; }

        public string Value { get; }

    }

    public class SectionView: ISectionView
    {
        //Связующее звено между xml объявлением секций и кодом доступа у сотрудника
        public IAccessSection AccessSection { get; set; }

        public IUserAccessOnSection UserSection { get; set; }

        public string Name => AccessSection.Name;
        public string Value => AccessSection.Params[UserSection.Value].Name;


        public SectionView(IAccessSection accessSection, IUserAccessOnSection userSection)
        {
            AccessSection = accessSection;
            UserSection = userSection;
        }
    }
}
