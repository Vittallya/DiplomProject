using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace AirClub.Model.Access
{
    public static class AccessCodeConverter
    {
        //Работает только со строкой и конвертированием ее в класс интерфейса IAccessParameter
        public static async Task<IEnumerable<IUserAccessOnSection>> ConvertFrom(string code, IList<IAccessSection> accessSections)
        {
            List<UserAccessOnSection> userSections = new List<UserAccessOnSection>();


            if (!IsAccessCodeCorrect(code, accessSections))
            {
                var result = await GetDefaultCode(accessSections);
                return result.Sections;
            }

            var codeArr = code.Split(';');

            foreach(var unit in codeArr)
            {
                if (TryConvertFromAccessCodeFragment(unit, out UserAccessOnSection section))
                {
                    userSections.Add(section);
                }
            }
            return userSections;
        }

        public static string FilePath { get; } = Directory.GetCurrentDirectory() + @"/AccessConfig.xml";

        public static async Task<IList<ISectionView>> ConvertFromCodeToSectionsView(string code, IList<IAccessSection> accessSections)
        {
            IList<IUserAccessOnSection> userDecoded;

            if (!TryConvertFromAccessCode(code, out userDecoded))
            {
                userDecoded = (await GetDefaultCode(accessSections)).Sections;
            }

            var sectionsView = new List<ISectionView>();

            foreach(var accSect in accessSections)
            {
                var userSect = userDecoded.FirstOrDefault(x => x.AccessIndex == accSect.Index);

                if(userSect == null)
                {
                    userSect = new UserAccessOnSection(0, accSect.Index);
                }

                var sectionView = new SectionView(accSect, userSect);
                sectionsView.Add(sectionView);

            }

            return sectionsView;
        }

        public static bool GetMax(IList<IAccessSection> accessSections, AccessSectionsIndex accessIndex, out int val)
        {
            val = 0;
            var sect = accessSections.FirstOrDefault(x => x.Index == (int)accessIndex);

            if(sect != null)
            {
                val = sect.Params.Last().Value;
                return true;
            }

            return false;

        }

        public static string ConvertFromSectionsViewToCode(IList<ISectionView> views)
        {
            var userSects = views.Select(x => x.UserSection).ToList();
            return ConvertToAccessCode(userSects);
        }

        public static IEnumerable<IAccessSection> Deserialize()
        {


            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                XmlSerializer xml = new XmlSerializer(typeof(AccessSection[]));


                var a = xml.Deserialize(fs);
                return (a as AccessSection[]);


               /* var sections = new AccessSection[]
                {
                    new AccessSection
                    {
                        Name = "Доступ к базе сотрудников",
                        Index = 10011,
                        Params = new AccessParam[]
                        {
                            new AccessParam("Нет доступа", 0),
                            new AccessParam("Только для чтения", 1),
                            new AccessParam("Чтение и редактирование", 2),
                        }
                    },
                    new AccessSection
                    {
                        Name = "Возможность добавления сотрудников",
                        Index = 10012,
                        Params = new AccessParam[]
                        {
                            new AccessParam("Нет", 0),
                            new AccessParam("Да", 1),

                        }
                    },
                };

                xml.Serialize(fs, sections);
                throw new NotImplementedException(); */

            }

        }

        private static bool  TryConvertFromAccessCodeFragment(string str, out UserAccessOnSection accessSection)
        {

            try
            {
                var strArr = str.Split('-');

                int index = Convert.ToInt32(strArr[0]);

                int value = Convert.ToInt32(strArr[1]);
                accessSection = new UserAccessOnSection(value, index);
                return true;
            }
            catch
            {
                accessSection = null;
                return false;
            }
        }

        private static bool TryConvertFromAccessCode(string code, out IList<IUserAccessOnSection> userDecoded)
        {
            userDecoded = null;

            if(code == null)
            {
                return false;
            }

            try
            {
                var sections = code.Split(';');

                userDecoded = new List<IUserAccessOnSection>();

                foreach(var sect in sections)
                {
                    if(TryConvertFromAccessCodeFragment(sect, out var decodedSect))
                    {
                        userDecoded.Add(decodedSect);
                    }
                }
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private static string ConvertToAccessCodeFragment(IUserAccessOnSection section)
        {
            string index = section.AccessIndex.ToString();
            string value = section.Value.ToString();

            return $"{index}-{value}";
        }

        public static string ConvertToAccessCode(IList<IUserAccessOnSection> userSections)
        {
            List<string> userSectsStr = new List<string>();

            foreach(var userSect in userSections)
            {
                userSectsStr.Add(ConvertToAccessCodeFragment(userSect));
            }
            return string.Join(";",userSectsStr);
        }

        public static async Task<dynamic> GetDefaultCode(IList<IAccessSection> sections)
        {
            List<string> userSectionsStr = new List<string>();
            List<IUserAccessOnSection> UserSections = new List<IUserAccessOnSection>();

            foreach(var param in sections)
            {
                var sect = new UserAccessOnSection(0, param.Index);
                UserSections.Add(sect);
                userSectionsStr.Add(ConvertToAccessCodeFragment(sect));
            }
            string acCode = string.Join(";", userSectionsStr);
            

            return new
            {
                Code = acCode,
                Sections = UserSections
            };
        }

        public static bool IsAccessCodeCorrect(string code, IList<IAccessSection> sections)
        {
            if(code == null)
            {
                return false;
            }

            var codeArr = code.Split(';');

            //if(codeArr.Length != sections.Count)
            //{
            //    //Если длина не соответсвует норме
            //    return false;
            //}

            foreach(var unit in codeArr)
            {
                if(TryConvertFromAccessCodeFragment(unit, out UserAccessOnSection sect))
                {
                    var param = sections.FirstOrDefault(x => x.Index == sect.AccessIndex);

                    if (param == null || sect.Value >= param.Params.Length)
                    {
                        //Если параметра с таким индексом нет, или значение выходит за пределы
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
