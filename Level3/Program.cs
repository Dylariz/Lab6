using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Level3
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Number1.Program.Start();
        }
    }
}

namespace Number1
{
    public static class Program
    {
        /// <summary>
        /// Результаты сессии содержат оценки 5 экзаменов по каждой
        /// группе. Определить средний балл для трех групп студентов одного
        /// потока и выдать список групп в порядке убывания среднего балла.
        /// Результаты вывести в виде таблицы с заголовком.
        /// </summary>
        public static void Start()
        {
            var groups = new List<Group>();
            groups.Add(Group.GenerateGroup(23, 5, (0, 10)));
            Thread.Sleep(50);
            groups.Add(Group.GenerateGroup(13, 5, (0, 10)));
            Thread.Sleep(50);
            groups.Add(Group.GenerateGroup(8, 5, (0, 10)));

            for (var index = 0; index < groups.Count; index++)
            {
                groups[index] = groups[index].DeleteStudentsWithMarkAndLower(2);
                groups[index] = groups[index].UpdateAvaregeMark();
                groups[index].SortStudentsByAverage();
                groups[index].PrintStudents();
            }

            Console.WriteLine();
            
            groups.Sort((x, y) => y.AverageMark.CompareTo(x.AverageMark));

            Console.WriteLine("Group\tAverage mark");
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Id}\t{group.AverageMark:F1}");
            }
        }
    }

    public struct Student
    {
        private static int _idContainer = 0;

        public int Id { get; private set; }
        public int[] Marks { get; private set; }
        public double AverageMark {get; private set;}

        public Student(int[] marks)
        {
            Id = _idContainer++;
            Marks = marks;
        }

        public Student UpdateAvarageMark()
        {
            double sum = 0;
            foreach (var mark in Marks)
                sum += mark;
            AverageMark = sum / Marks.Length;
            return this;
        }
    }

    public struct Group
    {
        private static int _idContainer = 0;
        public int Id { get; private set; }
        public List<Student> Students { get; private set; }
        public double AverageMark { get; private set; }

        public Group(List<Student> students)
        {
            Id = _idContainer++;
            Students = students;
        }
        
        public Group UpdateAvaregeMark()
        {
            double sum = 0;
            for (var index = 0; index < Students.Count; index++)
            {
                Students[index] = Students[index].UpdateAvarageMark();
                sum += Students[index].AverageMark;
            }

            AverageMark = sum / Students.Count;
            return this;
        }

        public Group DeleteStudentsWithMarkAndLower(int mark)
        {
            for (int i = 0; i < Students.Count; i++)
            {
                if (Students[i].Marks.Any(x => x <= mark))
                {
                    Students.RemoveAt(i);
                    i--;
                }
            }
            return this;
        }

        public void PrintStudents()
        {
            Console.WriteLine($"Группа №{Id}");
            foreach (var student in Students)
            {
                Console.WriteLine($"ID: {student.Id};\tAverage mark: {student.AverageMark:f1}");
            }

            Console.WriteLine();
        }

        public void SortStudentsByAverage()
        {
            Students.Sort((x, y) => y.AverageMark.CompareTo(x.AverageMark));
        }
        
        public static Group GenerateGroup(int countOfStudents, int countOfMarks, (int, int) marksRange)
        {
            List<Student> students = new List<Student>();
            for (int i = 0; i < countOfStudents; i++)
                students.Add(new Student(SupportMethods.GenerateArray(countOfMarks, marksRange)));
            return new Group(students);
        }
    }

    public static class SupportMethods
    {
        private static Random _random = new Random();

        public static int[] GenerateArray(int n, (int, int) range)
        {
            int[] array = new int[n];

            for (int i = 0; i < n; i++)
            {
                array[i] = _random.Next(range.Item1, range.Item2 + 1);
            }

            return array;
        }
    }
}

namespace Number4
{
    /// <summary>
    /// Лыжные гонки проводятся отдельно для двух групп участников. Результаты соревнований заданы в виде фамилий участников и
    /// их результатов в каждой группе. Расположить результаты соревнований в каждой группе в порядке занятых мест.
    /// Объединить результаты обеих групп с сохранением упорядоченности и вывести в виде таблицы с заголовком.
    /// </summary>
    internal static class Program
    {
        public static void Start()
        {
            var groups = new List<Group>();
            groups.Add(Group.GenerateGroup(10, (0, 10)));
            Thread.Sleep(50);
            groups.Add(Group.GenerateGroup(10, (0, 10)));


            Console.WriteLine();
            foreach (var group in groups)
            {
                Console.WriteLine($"Group {group.Id}");
                group.SortParticipants();
                group.PrintParticipants();
            }

            var mergedGroup = Group.MergeGroupsByResults(groups[0], groups[1]);
            Console.WriteLine("Merged group:");
            mergedGroup.PrintParticipants();
        }
    }

    public struct Participant
    {
        private static int _idContainer = 0;
        public string Name { get; private set; }
        public int Result { get; private set; }

        public Participant(int result)
        {
            Name = $"L{_idContainer++}";
            Result = result;
        }
    }

    public struct Group
    {
        private static int _idContainer = 0;
        public int Id { get; private set; }
        public List<Participant> Participants { get; private set; }

        public Group(List<Participant> participants)
        {
            Participants = participants;
            Id = _idContainer++;
        }

        public void SortParticipants()
        {
            Participants.Sort((x, y) => y.Result.CompareTo(x.Result));
        }

        public void PrintParticipants()
        {
            for (var index = 0; index < Participants.Count; index++)
            {
                var participant = Participants[index];
                Console.WriteLine($"{index + 1} место: {participant.Name} — {participant.Result}");
            }

            Console.WriteLine();
        }

        public static Group GenerateGroup(int countOfParticipants, (int, int) resultRange)
        {
            List<Participant> participants = new List<Participant>();
            for (int i = 0; i < countOfParticipants; i++)
            {
                participants.Add(new Participant(SupportMethods.GenerateNumber(resultRange)));
            }

            return new Group(participants);
        }
        
        public static Group MergeGroupsByResults(Group group1, Group group2)
        {
            var participants = new List<Participant>();
            for (int i = 0; i < Math.Max(group1.Participants.Count, group2.Participants.Count); i++)
            {
                if (i >= group1.Participants.Count)
                {
                    participants.Add(group2.Participants[i]);
                }
                else if (i >= group2.Participants.Count)
                {
                    participants.Add(group1.Participants[i]);
                }
                else if (group1.Participants[i].Result > group2.Participants[i].Result)
                {
                    participants.Add(group1.Participants[i]);
                    participants.Add(group2.Participants[i]);
                }
                else
                {
                    participants.Add(group2.Participants[i]);
                    participants.Add(group1.Participants[i]);
                }
            }

            return new Group(participants);
        }
    }

    public static class SupportMethods
    {
        private static Random _random = new Random();

        public static int GenerateNumber((int, int) range)
        {
            return _random.Next(range.Item1, range.Item2 + 1);
        }
    }
}

namespace Number6
{
    /// <summary>
    /// Японская радиокомпания провела опрос радиослушателей по трем вопросам:
    /// а) какое животное вы связываете с Японией и японцами?
    /// б) какая черта характера присуща японцам больше всего?
    /// в) какой неодушевленный предмет или понятие вы связываете с Японией?
    /// Большинство опрошенных прислали ответы на все или часть вопросов. Составить программу получения первых пяти наиболее часто
    /// встречающихся ответов по каждому вопросу и доли (%) каждого такого ответа. Предусмотреть необходимость сжатия столбца ответов в
    /// случае отсутствия ответов на некоторые вопросы.
    /// </summary>
    internal static class Program
    {
        public static void Start()
        {
        }
    }
}