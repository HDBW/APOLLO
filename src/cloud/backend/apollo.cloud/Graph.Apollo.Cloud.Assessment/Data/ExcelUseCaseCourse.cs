using Invite.Apollo.App.Graph.Assessment.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course.Enums;
using Invite.Apollo.App.Graph.Common.Models.Course;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using Serilog;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public class ExcelUseCaseCourse
    {


        /// <summary>
        /// Is a n:m mapper between courses and course contacts
        /// Ids 0 - 90 biwe
        /// Ids 0 - 29 biwe UseCase 1
        /// Ids 30 - 59 biwe UseCase 2
        /// Ids 60 - 90 biwe UseCase 3
        /// 
        /// Ids 100 - 190 bbw
        /// Ids 100 - 129 bbw UseCase 1
        /// Ids 130 - 159 bbw UseCase 2
        /// Ids 160 - 190 bbw UseCase 3
        /// 
        /// Ids 200 - 290 tüv
        /// Ids 200 - 229 bbw UseCase 1
        /// Ids 230 - 259 bbw UseCase 2
        /// Ids 260 - 290 bbw UseCase 3
        /// </summary>

        public enum EduProviderId
            {
                Biwe = 0,
                Bbw = 1,
                Tuev = 2,
                Bfz =3
            }

        public static Dictionary<int, string> providerNames = new Dictionary<int, string>()
        {
            {(int)EduProviderId.Biwe, "Biwe"}, {(int)EduProviderId.Bbw, "bbw"}, {(int)EduProviderId.Tuev, "TüV Rheinland Akademie" },  {(int)EduProviderId.Bfz, "bfz" }
        };

        public static Dictionary<string, int> getProviderIdByName = new Dictionary<string, int>()
        {
            {"Biwe", (int)EduProviderId.Biwe}, {"bbw", (int)EduProviderId.Bbw}, {"Tuev", (int)EduProviderId.Tuev },  {"bfz", (int)EduProviderId.Bfz }
        };


        /// <summary>
        /// Get List of <c>Course</c> Objects by Usecasenumber int
        /// </summary>
        public Dictionary<int, List<CourseItem>> CoursesByUseCaseId = new Dictionary<int, List<CourseItem>>();

        /// <summary>
        /// Get List of <c>CourseContact</c> Objects by Usecasenumber int
        /// </summary>
        public Dictionary<int, List<CourseContact>> ContactsByUseCaseId = new Dictionary<int, List<CourseContact>>();

        public Dictionary<int, List<CourseContactRelation>> CourseContactRelationByUseCaseId = new();

        public Dictionary<long, CourseContactRelation> CourseContactRelationsByCourseId = new();

        public Dictionary<int, EduProviderItem> ProviderList = new()
        {
            //TODO: Set Logo!
            {
                (int)EduProviderId.Biwe,
                new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = providerNames[(int)EduProviderId.Biwe],
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id =(int)EduProviderId.Biwe,
                    Website = new Uri("https://www.biwe-bbq.de/")
                }

            },
            {
                (int)EduProviderId.Bbw,
                new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = providerNames[(int)EduProviderId.Bbw],
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = (int)EduProviderId.Bbw,
                    Website = new Uri("https://www.bbw-weiterbildung.de/")
                }
            },
            {
                (int)EduProviderId.Tuev,
                new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = providerNames[(int)EduProviderId.Tuev],
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = (int)EduProviderId.Tuev,
                    Website = new Uri("https://akademie.tuv.com/")
                }
            },
            {
                (int)EduProviderId.Bfz,
                new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = providerNames[(int)EduProviderId.Bfz],
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = (int)EduProviderId.Bfz,
                    Website = new Uri("https://www.bfz.de/")
                }
            },
            {
                4,
                new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = "BBQ Bildung und Berufliche Qualifizierung gGmbH",
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = 4,
                    Website = new Uri("https://www.biwe-bbq.de/")
                }
            },
            {
                5,
                new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = "Berufliche Fortbildungszentren der Bayerischen Wirtschaft (bfz) gGmbH",
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = 5,
                    Website = new Uri("https://www.bfz.de/")
                }
            },
            {
                6, new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = "TÜV Rheinland Akademie GmbH",
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = 6,
                    Website = new Uri("https://akademie.tuv.com/")
                }
            },
            {
                7, new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = "bbw it akademie bayern",
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = 7,
                    Website = new Uri("https://www.bbw-seminare.de/it-akademie/")
                }
            },
            {
                8, new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = "Bildungswerk der Bayerischen Wirtschaft (bbw) gemeinnützige GmbH",
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = 8,
                    Website = new Uri("https://www.bbw-seminare.de/")
                }
            },
            {
                9, new EduProviderItem()
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Name = "Bildungswerk der Baden-Württembergischen Wirtschaft e.V.",
                    Description = "",
                    Logo = new Uri("https://invite-apollo.app/TODO"),
                    Id = 9,
                    Website = new Uri("https://www.biwe-akademie.de/")
                }
            }
            

        };

        public Dictionary<long, CourseContact> ContactsById = new();

        public Dictionary<long, CourseItem> CourseById = new();

        public Dictionary<long, List<CourseAppointment>> AppointmentsByCourseId = new();

        public Dictionary<int, List<CourseAppointment>> AppointmentsByUseCaseId = new();

        private CourseContact getInstructor(int providerId)
        {
            
            CourseContact contact = null;

            switch (providerId)
            {
                case (int)EduProviderId.Biwe:
                    contact = ContactsById[0];
                    break;
                case (int)EduProviderId.Bbw:
                    contact = ContactsById[100];
                    break;
                case (int)EduProviderId.Bfz:
                    contact = ContactsById[100];
                    break;
                case (int)EduProviderId.Tuev:
                    contact = ContactsById[200];
                    break;

            }

            return contact;
        }

        public ExcelUseCaseCourse(string filename)
        {

            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Expected File not found", filename);
            }

            //Default Instructor Contacts
            ContactsById.Add(0, new CourseContact()
                                    {
                                        Id = 0,
                                        ContactMail = "info-bbq@biwe.de",
                                        ContactName = "BBQ Bildung und Berufliche Qualifizierung gGmbH",
                                        ContactPhone = "+49 (0)711 135340-0",
                                        Ticks = DateTime.Now.Ticks,
                                        Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                                        Url = new Uri("https://www.biwe-bbq.de/")
                                    });
            ContactsById.Add(100, new CourseContact()
            {
                Id = 100,
                ContactMail = "mira.bernhart@bbw.de",
                ContactName = "Mira Bernhart",
                ContactPhone = "+499317973261",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://www.bbw-seminare.de/inhouse-angebote/")
            });

            ContactsById.Add(200, new CourseContact()
            {
                Id = 200,
                ContactMail = "servicecenter@de.tuv.com",
                ContactName = "Service Center",
                ContactPhone = "+4980013535577",
                Ticks = DateTime.Now.Ticks,
                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                Url = new Uri("https://akademie.tuv.com/ueber-die-akademie")
            });


            for (int i = 1; i < 4; i++)
            {
                AppointmentsByUseCaseId.Add(i, new());
                ContactsByUseCaseId.Add(i, new());
                CoursesByUseCaseId.Add(i, new());
                CourseContactRelationByUseCaseId.Add(i, new());
            }

            LoadCoursefromExcelWorkbook(filename);


            //CourseContactRelations = new();

            ////course biwe setup tq lager
            //CourseAppointment appointment = Appointments[0];
            //CourseItem course = CourseList[0];
            //SetCourseAppointmentRelations(appointment, course);

            //appointment = Appointments[1];
            //course = CourseList[0];
            //SetCourseAppointmentRelations(appointment, course);

            //appointment = Appointments[2];
            //course = CourseList[0];
            //SetCourseAppointmentRelations(appointment, course);

            //appointment = Appointments[3];
            //course = CourseList[0];
            //SetCourseAppointmentRelations(appointment, course);


            //List<CourseItem> usecaseCourseLists = new List<CourseItem>();


            //for (int j = 0; j < 2; j++)
            //    SetContactCourseRelations(course, Contacts[j]);


            ////TODO: Add other UseCases and Courses
            ////Usecase 1 
            //for (int i = 0; i < 20; i++)
            //{
            //    if (CourseList.ContainsKey(i))
            //        usecaseCourseLists.Add(CourseList[i]);
            //}

            //for (int i = 60; i < 80; i++)
            //{
            //    if (CourseList.ContainsKey(i))
            //        usecaseCourseLists.Add(CourseList[i]);
            //}

            //usecaseCourses.Add(0, usecaseCourseLists);

            //System.Console.WriteLine(ProviderList.Count);


        }



        //TODO: ändern wenn mehr als ein Kontakt pro Kurs! -> nicht instructor für Dezember
        private CourseContactRelation SetContactCourseRelations(CourseItem course, CourseContact contact)
        {

            CourseContactRelation result = null;

            if (!CourseContactRelationsByCourseId.ContainsKey(course.Id))
            {
                result= new CourseContactRelation
                {
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Ticks = DateTime.Now.Ticks,
                    Id = CourseContactRelationsByCourseId.Count,
                    CourseContactId = contact.Id,
                    CourseId = course.Id
                };
                CourseContactRelationsByCourseId.Add(course.Id, result);

                //CourseContactRelationByUseCaseId[useCaseId].Add(courseContactRelation);

            }

            return result;
        }

        private CourseContact CreateContact(string name, string mail, string tel, long id, string url, CourseItem course)
        {
            CourseContact contact = new CourseContact();

            if (ContactsById.ContainsKey(id))
                contact=  ContactsById[id];
            else
            {
                Uri Url = string.Empty.Equals(url)  ? new Uri($"https://invite-apollo.app/{Guid.NewGuid()}") : new Uri(url);
                contact = new CourseContact()
                {
                    Id = id,
                    Ticks = DateTime.Now.Ticks,
                    Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                    Url = Url,
                    ContactMail = mail,
                    ContactPhone = tel,
                    ContactName = name
                };
                ContactsById.Add(id, contact);
            }
            //SetContactCourseRelations(course, contact);

            return contact;


        }



        private void LoadCoursefromExcelWorkbook(string filename)
        {
            
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = null;


            #region Read from CSV into items

            try
            {
                xlWorkbook = xlApp.Workbooks.Open(filename);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[2];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                var rowCount = xlRange.Rows.Count;
                var colCount = xlRange.Columns.Count;


                //this is pointer stuff for iterating over the excel columns and rows
                for (int i = 2; i <= rowCount; i++)
                {
                    CourseItem item = new();

                    var useCaseId = xlRange.Cells[i, ExcelCourseColumnIndex.UseCase].Value2 != null
                        ? (int)Convert.ToInt32(xlRange.Cells[i, ExcelCourseColumnIndex.UseCase].Value2.ToString())
                        : default;


                    item.Id = xlRange.Cells[i, ExcelCourseColumnIndex.CourseId].Value2 != null
                        ? (long)Convert.ToInt64(xlRange.Cells[i, ExcelCourseColumnIndex.CourseId].Value2.ToString())
                        : default;

                   

                    item.UnPublishingDate = (xlRange.Cells[i, ExcelCourseColumnIndex.UnPublishingDate].Value2 != null)
                        ? DateTime.Parse(xlRange.Cells[i, ExcelCourseColumnIndex.UnPublishingDate].Value2.ToString())
                        : null;

                 
                    item.Duration = (xlRange.Cells[i, ExcelCourseColumnIndex.Duration].Value2 != null)
                            ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.Duration].Value2.ToString()
                            : string.Empty;


                    //Assumtion Excel not null
                    string? CourseProvider = xlRange.Cells[i, ExcelCourseColumnIndex.CourseProviderId].Value2.ToString();
                    item.CourseProviderId = GetEduProvider(CourseProvider);
                    string? TrainingProvider = xlRange.Cells[i, ExcelCourseColumnIndex.TrainingProviderId].Value2.ToString();
                    item.TrainingProviderId = GetTrainingProvider(TrainingProvider);
                    //item.CourseProviderId = xlRange.Cells[i, ExcelCourseColumnIndex.CourseProviderId].Value2 != null
                    //    ? (long)Convert.ToInt64(xlRange.Cells[i, ExcelCourseColumnIndex.CourseProviderId].Value2.ToString())
                    //    : default;
                    //item.TrainingProviderId = xlRange.Cells[i, ExcelCourseColumnIndex.TrainingProviderId].Value2 != null
                    //    ? (long)Convert.ToInt64(xlRange.Cells[i, ExcelCourseColumnIndex.TrainingProviderId].Value2.ToString())
                    //    : default;

                    string instructorStr = xlRange.Cells[i, ExcelCourseColumnIndex.InstructorId].Value2.ToString();
                    item.InstructorId = getInstructor(getProviderIdByName[instructorStr]).Id;
                    //item.InstructorId = xlRange.Cells[i, ExcelCourseColumnIndex.InstructorId].Value2 != null
                    //    ? (long)Convert.ToInt64(xlRange.Cells[i, ExcelCourseColumnIndex.InstructorId].Value2.ToString())
                    //    : default;

                    //TagType
                    CourseTagType tag = CourseTagType.Unknown;
                    string strTag = (xlRange.Cells[i, ExcelCourseColumnIndex.CourseTagType].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.CourseTagType].Value2.ToString()
                        : string.Empty;

                    if(Enum.TryParse<CourseTagType>(strTag, out tag))
                        item.CourseTagType = tag;
                    else
                        item.CourseTagType = CourseTagType.Unknown;

                    //Type
                    CourseType type = CourseType.Unknown;
                    string strType = (xlRange.Cells[i, ExcelCourseColumnIndex.CourseType].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.CourseType].Value2.ToString()
                        : string.Empty;

                    if (Enum.TryParse<CourseType>(strType, out type))
                        item.CourseType = type;
                    else
                        item.CourseType = CourseType.Unknown;


                    //OccurrenceType
                    OccurrenceType occurrenceType = OccurrenceType.FullTime;
                    string strOccurrenceType = (xlRange.Cells[i, ExcelCourseColumnIndex.Occurrence].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.Occurrence].Value2.ToString()
                        : string.Empty;

                    if (Enum.TryParse<OccurrenceType>(strOccurrenceType, out occurrenceType))
                        item.Occurrence = occurrenceType;
                    else
                        item.Occurrence = OccurrenceType.FullTime;

                    item.CourseUrl = new Uri((xlRange.Cells[i, ExcelCourseColumnIndex.CourseUrl].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.CourseUrl].Value2.ToString()
                        : $"https://invite-apollo.app/{Guid.NewGuid()}");

                    item.ExternalId = (xlRange.Cells[i, ExcelCourseColumnIndex.ExternalId].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.ExternalId].Value2.ToString()
                        : string.Empty;

                    item.Language = (xlRange.Cells[i, ExcelCourseColumnIndex.Language].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.Language].Value2.ToString()
                        : string.Empty;

                    item.Title = (xlRange.Cells[i, ExcelCourseColumnIndex.Title].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.Title].Value2.ToString()
                        : string.Empty;
                    item.ShortDescription = (xlRange.Cells[i, ExcelCourseColumnIndex.ShortDescription].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.ShortDescription].Value2.ToString()
                        : string.Empty;
                    item.Description = (xlRange.Cells[i, ExcelCourseColumnIndex.Description].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.Description].Value2.ToString()
                        : string.Empty;
                    item.TargetGroup = (xlRange.Cells[i, ExcelCourseColumnIndex.TargetGroup].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.TargetGroup].Value2.ToString()
                        : string.Empty;
                    item.PreRequisitesDescription = (xlRange.Cells[i, ExcelCourseColumnIndex.PreRequisitesDescription].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.PreRequisitesDescription].Value2.ToString()
                        : string.Empty;
                    item.LearningOutcomes = (xlRange.Cells[i, ExcelCourseColumnIndex.LearningOutcomes].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.LearningOutcomes].Value2.ToString()
                        : string.Empty;
                    item.Benefits = (xlRange.Cells[i, ExcelCourseColumnIndex.Benefits].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.Benefits].Value2.ToString()
                        : string.Empty;

                    item.LoanOptions = (xlRange.Cells[i, ExcelCourseColumnIndex.LoanOptions].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.LoanOptions].Value2.ToString()
                        : string.Empty;

                    item.Skills = (xlRange.Cells[i, ExcelCourseColumnIndex.Skills].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.Skills].Value2.ToString()
                        : string.Empty;
                    item.KeyPhrases = (xlRange.Cells[i, ExcelCourseColumnIndex.KeyPhrases].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.KeyPhrases].Value2.ToString()
                        : string.Empty;

                    item.Price = xlRange.Cells[i, ExcelCourseColumnIndex.Price].Value2 != null
                        ? (decimal)Convert.ToDecimal(xlRange.Cells[i, ExcelCourseColumnIndex.Price].Value2.ToString())
                        : default;




                    //Contact Stuff
                    var ContactId = xlRange.Cells[i, ExcelCourseColumnIndex.ContactId].Value2 != null
                        ? (long)Convert.ToInt64(xlRange.Cells[i, ExcelCourseColumnIndex.ContactId].Value2.ToString())
                        : -1;

                    string ContactMail = xlRange.Cells[i, ExcelCourseColumnIndex.ContactMail].Value2 != null
                                ? xlRange.Cells[i, ExcelCourseColumnIndex.ContactMail].Value2.ToString()
                                : string.Empty;

                    //string ContactPhone = xlRange.Cells[i, ExcelCourseColumnIndex.ContactPhone].Value2.ToString();
                    //string contactName = xlRange.Cells[i, ExcelCourseColumnIndex.ContactName].Value2.ToString();

                    string ContactPhone = xlRange.Cells[i, ExcelCourseColumnIndex.ContactPhone].Value2 != null
                        ? xlRange.Cells[i, ExcelCourseColumnIndex.ContactPhone].Value2.ToString()
                        : string.Empty;
                    string contactName = xlRange.Cells[i, ExcelCourseColumnIndex.ContactName].Value2 != null
                        ? xlRange.Cells[i, ExcelCourseColumnIndex.ContactName].Value2.ToString()
                        : string.Empty;

                    var ContactUrl = (xlRange.Cells[i, ExcelCourseColumnIndex.ContactUrl].Value2 != null)
                        ? (string)xlRange.Cells[i, ExcelCourseColumnIndex.ContactUrl].Value2.ToString()
                        : string.Empty;

                    CourseContact contact = CreateContact(contactName, ContactMail, ContactPhone, ContactId, ContactUrl, item);
                    var relation = SetContactCourseRelations(item, contact);
                    CourseContactRelationByUseCaseId[useCaseId].Add(relation);
                    ContactsByUseCaseId[useCaseId].Add(contact);
                    
                    

                    
                    




                    //AppoointmentStuff
                    for (int j = (int)ExcelCourseColumnIndex.TerminStart1; j <= (int)ExcelCourseColumnIndex.TerminEnde3; j=j+2)
                    {
                        
                        

                        CourseAppointment appointment =
                            new CourseAppointment()
                            {
                                Id = AppointmentCounter++,
                                CourseId = item.Id,
                                Ticks = DateTime.Now.Ticks,
                                Schema = new Uri($"https://invite-apollo.app/{Guid.NewGuid()}"),
                                StartDate = (xlRange.Cells[i, j].Value2 != null)
                                    ? DateTime.Parse(xlRange.Cells[i, j].Value2.ToString()) : null,
                                EndDate = (xlRange.Cells[i, j + 1].Value2 != null)
                                    ? DateTime.Parse(xlRange.Cells[i, j + 1].Value2.ToString()) : null
                                
                            };
                        if ((xlRange.Cells[i, j].Value2 != null))
                        {
                            //appointment
                            if(AppointmentsByCourseId.ContainsKey(item.Id))
                                AppointmentsByCourseId[item.Id].Add(appointment);
                            else
                            {
                                AppointmentsByCourseId.Add(item.Id, new(){appointment});
                            }
                            AppointmentsByUseCaseId[useCaseId].Add(appointment);


                            


                        }


                    }
                    CourseById.Add(item.Id, item);
                    CoursesByUseCaseId[useCaseId].Add(item);
                    
                }

            }
            catch (Exception e)
            {
                //xlWorkbook.Close();
                //TODO: Check if Finalization queue is reached when exception occurs
                throw new Exception("AHHHHHH Excel Workbook Stuff", e);
            }
            finally
            {
                xlWorkbook.Close();
            }

            #endregion
        }

        private long GetTrainingProvider(string trainingProvider)
        {
            var result = ProviderList.Where(x => x.Value.Name.Equals(trainingProvider)).ToList();
            if (result.Count>0)
            {
                return result.FirstOrDefault().Value.Id;
            }
            else
            {
                return -1;
            }

        }

        public int AppointmentCounter { get; set; }

        private long GetEduProvider(string? courseProvider)
        {
            
            int providerid = 0;
            if (courseProvider != null && getProviderIdByName.ContainsKey(courseProvider))
                providerid = getProviderIdByName[courseProvider];
            else
            {
                return -1;
            }
            

            return Convert.ToInt64(providerid);

            throw new NotImplementedException();
        }
    }
}
