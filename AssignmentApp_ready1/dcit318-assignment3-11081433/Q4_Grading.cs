using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment.Q4
{
    public class Student
    {
        public int Id; public string FullName =string.Empty; public int Score;
        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100) return "A";
            if (Score >= 70) return "B";
            if (Score >= 60) return "C";
            if (Score >= 50) return "D";
            return "F";
        }
    }

    public class InvalidScoreFormatException : Exception { public InvalidScoreFormatException(string msg) : base(msg) { } }
    public class MissingFieldException : Exception { public MissingFieldException(string msg) : base(msg) { } }

    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            List<Student> students = new();

            using var reader = new StreamReader(inputFilePath);
            string? line;
            int lineno = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineno++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',', StringSplitOptions.TrimEntries);
                if (parts.Length != 3)
                    throw new MissingFieldException($"Line {lineno}: Expected 3 fields, got {parts.Length}");

                if (!int.TryParse(parts[0], out int id))
                    throw new MissingFieldException($"Line {lineno}: Invalid or missing Id.");

                string fullName = parts[1];

                if (!int.TryParse(parts[2], out int score))
                    throw new InvalidScoreFormatException($"Line {lineno}: Score is not a valid integer.");

                students.Add(new Student { Id = id, FullName = fullName, Score = score });
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using var writer = new StreamWriter(outputFilePath, false);
            foreach (var s in students)
            {
                writer.WriteLine($"{s.FullName} (ID: {s.Id}): Score = {s.Score}, Grade = {s.GetGrade()}");
            }
        }
    }

    public class GradingDemo
    {
        public void Run()
        {
            Console.WriteLine("=== Q4: Grading System ===");

            string baseDir = Path.Combine(Directory.GetCurrentDirectory(), "q4_io");
            Directory.CreateDirectory(baseDir);
            string input = Path.Combine(baseDir, "students.txt");
            string output = Path.Combine(baseDir, "report.txt");

            File.WriteAllLines(input, new[]
            {
                "101,Alice Smith,84",
                "102,Kwame Mensah,73",
                "103,Ama Adwoa,59",
                "104,John Doe,42"
            });

            var proc = new StudentResultProcessor();

            try
            {
                var students = proc.ReadStudentsFromFile(input);
                proc.WriteReportToFile(students, output);
                Console.WriteLine($"Report written to: {output}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"[File Not Found] {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"[Invalid Score] {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"[Missing Field] {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unexpected Error] {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}
