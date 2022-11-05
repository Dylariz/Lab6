using System;
using System.Collections.Generic;
using System.Threading;

namespace Number1
{
    /// <summary>
    /// !!! Это задание третьего уровня из 6-й лабороторной работы.
    /// Результаты сессии содержат оценки 5 экзаменов по каждой
    /// группе. Определить средний балл для трех групп студентов одного
    /// потока и выдать список групп в порядке убывания среднего балла.
    /// Результаты вывести в виде таблицы с заголовком.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<Group> groups = new List<Group>();
            groups.Add(Group.GenerateGroup(23, 5, (0, 10)));
            Thread.Sleep(50);
            groups.Add(Group.GenerateGroup(13, 5, (0, 10)));
            Thread.Sleep(50);
            groups.Add(Group.GenerateGroup(8, 5, (0, 10)));
            
            groups.Sort((x, y) => y.GetAverageMark().CompareTo(x.GetAverageMark()));
            
            Console.WriteLine("Group\tAverage mark");
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Id}\t{group.GetAverageMark()}");
            }
        }
    }
    
    public struct Student
    {
        private static int idContainer = 0;
        
        public int Id { get; private set; }
        public int[] Marks { get; private set; }
        
        public double AverageMark
        {
            get
            {
                double sum = 0;
                foreach (var mark in Marks)
                    sum += mark;
                return sum / Marks.Length;
            }
        }
        
        public Student(int[] marks)
        {
            Id = idContainer++;
            Marks = marks;
        }
    }
    
    public struct Group
    {
        private static int idContainer = 0;
        public int Id { get; private set; }
        public Student[] Students { get; private set; }
        
        public Group(Student[] students)
        {
            Id = idContainer++;
            Students = students;
        }
        
        public double GetAverageMark()
        {
            double sum = 0;
            foreach (var student in Students)
                sum += student.AverageMark;
            return sum / Students.Length;
        }
        
        public static Group GenerateGroup(int countOfStudents, int countOfMarks, (int, int) marksRange)
        {
            Student[] students = new Student[countOfStudents];
            for (int i = 0; i < countOfStudents; i++)
                students[i] = new Student(Support.GenerateArray(countOfMarks, marksRange));
            return new Group(students);
        }
    }

    public static class Support
    {
        public static int[] GenerateArray(int n, (int, int) range)
        {
            int[] array = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(range.Item1, range.Item2 + 1);
            }

            return array;
        }
    }
}