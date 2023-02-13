using System;
using System.Collections.Generic;

namespace Level2
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Number2.Program.Start();
        }
    }
}

namespace Number2
{
    /// <summary>
    /// Группа учащихся подготовительного отделения сдает выпускные экзамены по трем предметам (математика, физика, русский язык).
    /// Учащийся, получивший «2», сразу отчисляется. Вывести список учащихся, успешно сдавших экзамены, в порядке убывания полученного
    /// ими среднего балла по результатам трех экзаменов.
    /// </summary>
    internal static class Program
    {
        static Random _random = new Random();

        public static void Start()
        {
            int n = 20;
            Group group = new Group();
            string[] names =
            {
                "Рокоссовский",
                "Пермяков",
                "Горемыкин",
                "Степанков",
                "Чиграков",
                "Чукчов",
                "Моренов",
                "Черняков",
                "Сусоев",
                "Львов",
                "Боголюбов",
                "Трусов",
                "Денисов",
                "Любов",
                "Куклачёв",
                "Кораблин",
                "Шульга",
                "Изюмов",
                "Дёмин",
                "Сутулин"
            };
            for (int i = 0; i < n; i++)
            {
                group.AddStudent(new Student(names[_random.Next(0, names.Length)], _random.Next(2, 6),
                    _random.Next(2, 6), _random.Next(2, 6)));
            }

            group.DeleteStudentsWithMark(2);
            group.SortStudentsByAverage();
            group.PrintStudents();
        }
    }

    public struct Student
    {
        public string Name { get; private set; }
        public Dictionary<string, int> Marks { get; private set; }

        public double Average
        {
            get
            {
                double sum = 0;
                foreach (var mark in Marks)
                {
                    sum += mark.Value;
                }

                return Math.Round(sum / Marks.Count, 1);
            }
        }

        public Student(string name, int math, int physics, int russian) : this(name, new Dictionary<string, int>())
        {
            Marks.Add("Math", math);
            Marks.Add("Physics", physics);
            Marks.Add("Russian", russian);
        }

        public Student(string name, Dictionary<string, int> marks)
        {
            Name = name;
            Marks = marks;
        }
    }

    public struct Group
    {
        private static int _idContainer = 0;
        public int Id { get; private set; }

        public List<Student> Students { get; private set; }

        public Group() : this(new List<Student>())
        {
        }

        public Group(List<Student> students)
        {
            Id = _idContainer++;
            Students = students;
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        public void DeleteStudentsWithMark(int mark)
        {
            for (int i = 0; i < Students.Count; i++)
            {
                if (Students[i].Marks.ContainsValue(mark))
                {
                    Students.RemoveAt(i);
                    i--;
                }
            }
        }

        public void PrintStudents()
        {
            foreach (var student in Students)
            {
                Console.WriteLine($"Фамилия: {student.Name}; \tСредняя оценка: {student.Average}");
            }
        }

        public void SortStudentsByAverage()
        {
            Students.Sort((x, y) => y.Average.CompareTo(x.Average));
        }
    }
}