using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment.Q2
{
    public class Repository<T>
    {
        private readonly List<T> items = new();
        public void Add(T item) => items.Add(item);
        public List<T> GetAll() => new(items);
        public T? GetById(Func<T, bool> predicate) => items.FirstOrDefault(predicate);
        public bool Remove(Func<T, bool> predicate)
        {
            var idx = items.FindIndex(x => predicate(x));
            if (idx >= 0) { items.RemoveAt(idx); return true; }
            return false;
        }
    }

    public class Patient
    {
        public int Id; public string Name; public int Age; public string Gender;
        public Patient(int id, string name, int age, string gender) { Id = id; Name = name; Age = age; Gender = gender; }
        public override string ToString() => $"Patient(Id={Id}, Name={Name}, Age={Age}, Gender={Gender})";
    }

    public class Prescription
    {
        public int Id; public int PatientId; public string MedicationName; public DateTime DateIssued;
        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued) { Id = id; PatientId = patientId; MedicationName = medicationName; DateIssued = dateIssued; }
        public override string ToString() => $"Prescription(Id={Id}, PatientId={PatientId}, Med={MedicationName}, Date={DateIssued:d})";
    }

    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public List<Prescription> GetPrescriptionsByPatientId(int patientId) =>
            _prescriptionMap.TryGetValue(patientId, out var list) ? list : new List<Prescription>();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "Alice Smith", 30, "F"));
            _patientRepo.Add(new Patient(2, "Kwame Mensah", 42, "M"));
            _patientRepo.Add(new Patient(3, "Ama Adwoa", 25, "F"));

            _prescriptionRepo.Add(new Prescription(101, 1, "Amoxicillin", DateTime.Today.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(102, 1, "Ibuprofen", DateTime.Today.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(103, 2, "Lisinopril", DateTime.Today.AddDays(-20)));
            _prescriptionRepo.Add(new Prescription(104, 3, "Cetirizine", DateTime.Today.AddDays(-1)));
            _prescriptionRepo.Add(new Prescription(105, 2, "Vitamin D", DateTime.Today));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap = _prescriptionRepo.GetAll().GroupBy(p => p.PatientId).ToDictionary(g => g.Key, g => g.ToList());
        }

        public void PrintAllPatients()
        {
            foreach (var p in _patientRepo.GetAll()) Console.WriteLine(p);
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            var rx = GetPrescriptionsByPatientId(id);
            if (rx.Count == 0) { Console.WriteLine($"No prescriptions found for PatientId={id}"); return; }
            Console.WriteLine($"Prescriptions for PatientId={id}:");
            foreach (var p in rx) Console.WriteLine("  " + p);
        }

        public void Run()
        {
            Console.WriteLine("=== Q2: Healthcare System ===");
            SeedData(); BuildPrescriptionMap();
            Console.WriteLine("-- All Patients --"); PrintAllPatients();
            Console.WriteLine("-- Prescriptions for PatientId=2 --"); PrintPrescriptionsForPatient(2);
            Console.WriteLine();
        }
    }
}
