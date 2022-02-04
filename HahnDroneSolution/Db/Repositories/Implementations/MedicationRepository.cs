using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Db.Repositories.Interfaces;
using System.Linq;

namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class MedicationRepository : Repository<Medication>, IMedicationRepository
    {
        
        public MedicationRepository(HahnDroneDBContext context) : base(context)
        {
            SeedData();
        }

        public IQueryable<Medication> Medications => _context.Medications.OrderByDescending(x => x.MedicationID).AsQueryable();

        public void SeedData()
        {

            if (!_context.Medications.Any())
            {
                _context.Medications.Add(new Medication() { Name = "Hydrocodone", Code = "HYD123", Weight = 200, Image = "https://c8.alamy.com/comp/2A32WJP/hydrocodone-concept-image-with-molecule-and-chemical-formula-2A32WJP.jpg" });
                _context.Medications.Add(new Medication() { Name = "Generic Synthroid", Code = "GESY123", Weight = 150, Image = "https://c8.alamy.com/comp/CC837D/packet-and-strip-of-levothyroxine-tablets-for-the-treatment-of-hypothyroidism-CC837D.jpg" });
                _context.Medications.Add(new Medication() { Name = "Azithromycin", Code = "AZYC123", Weight = 50, Image = "https://c8.alamy.com/comp/2EK8RB6/boxes-of-plaquenil-hydroxychloroquine-and-azithromycin-french-packaging-drugs-that-form-the-basis-of-a-controversial-treatment-for-covid-19-2EK8RB6.jpg" });
                _context.Medications.Add(new Medication() { Name = "Amoxicillin", Code = "AMON123", Weight = 100, Image = "https://c8.alamy.com/comp/BHM36C/amoxicillin-capsules-and-packet-BHM36C.jpg" });
                _context.Medications.Add(new Medication() { Name = "Nexium", Code = "NEXM123", Weight = 250, Image = "https://pharmaceutical-journal.com/wp-content/uploads/2021/01/nexium-control-box-16-927x617.jpg" });

                _context.SaveChanges();
            }

        }

    }
}
