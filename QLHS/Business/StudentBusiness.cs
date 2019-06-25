using QLHS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QLHS.Business
{
    public class StudentBusiness
    {
        private XmlSerializer xmlSerializer;
        private List<Student> students;
        private List<StudentInfo> studentInfos;
        private AddressBusiness addressBusiness;

        public StudentBusiness()
        {
            addressBusiness = AddressBusiness.GetInstance();
            xmlSerializer = new XmlSerializer(typeof(List<Student>));
            if (studentInfos == null || studentInfos.Count == 0)
            {
                studentInfos = GetAllStudentInfos();
            }
            //students = new List<NewStudent>();
            //students.Add(new NewStudent { Id = 1, Name = "Nguyen Van A", Birthday = DateTime.Now, Gender = 1, AddressId = 78 });
            //SaveStudents();
        }

        public List<StudentInfo> GetAllStudentInfos()
        {
            GetAllStudents();
            List<StudentInfo> studentInfo = new List<StudentInfo>();
            foreach (var student in students)
            {
                AddressItem addressItem = addressBusiness.GetAddressItem(student.AddressId);
                studentInfo.Add(new StudentInfo { Student = student, Address = addressItem });
            }
            return studentInfo;
        }

        public List<Student> GetAllStudents()
        {
            try
            {
                FileStream stream = System.IO.File.OpenRead("App_Data/students.xml");
                students = (List<Student>)xmlSerializer.Deserialize(stream);
                stream.Close();
            }
            catch (Exception)
            {
                students = new List<Student>();
                SaveStudents();
            }

            return students;
        }

        //public List<NewStudent> GetAllStudents()
        //{
        //    FileStream stream = System.IO.File.OpenRead("App_Data/new_students.xml");
        //    students = (List<NewStudent>)xmlSerializer.Deserialize(stream);
        //    stream.Close();
        //    return students;
        //}

        public Student GetStudentById(long id)
        {
            GetAllStudents();
            return students.FirstOrDefault(s => s.Id == id);
        }

        private void SaveStudents()
        {
            if (students == null) students = new List<Student>();
            FileStream stream = new FileStream("App_Data/students.xml", FileMode.Create);
            xmlSerializer.Serialize(stream, students);
            stream.Close();
        }

        public void InsertStudent(Student student)
        {
            students = GetAllStudents();
            if (students == null || students.Count == 0)
            {
                student.Id = 0;
            }
            else { student.Id = students.Last().Id + 1; }
            students.Add(student);
            SaveStudents();
        }

        public void InsertStudents(List<Student> students)
        {
            if (students == null || students.Count == 0) return;
            students = GetAllStudents();
            long lastIndex = 0;
            if (students == null || students.Count == 0)
            {
                lastIndex = 0;
            }
            else { lastIndex = students.Last().Id; }
            for (int i = 0; i < students.Count; i++)
            {
                students[i].Id = i + lastIndex;
            }
            students.AddRange(students);
            SaveStudents();
        }

        public void UpdateStudent(Student student)
        {
            GetAllStudents();
            int index = students.FindIndex(s => s.Id == student.Id);
            if (index >= 0)
            {
                students[index] = student;
                SaveStudents();
            }
        }

        public void DeleteStudent(Student student)
        {
            GetAllStudents();
            if (students.Contains(student))
            {
                students.Remove(student);
                SaveStudents();
            }
        }

        public void DeleteStudent(long id)
        {
            GetAllStudents();
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                students.Remove(student);
                SaveStudents();
            }
        }

        public void DeleteStudents(List<Student> students)
        {
            GetAllStudents();
            foreach (var student in students)
            {
                if (this.students.Contains(student))
                {
                    students.Remove(student);
                }
            }
            SaveStudents();
        }

        public void DeleteAll()
        {
            students.Clear();
            SaveStudents();
        }
    }
}
