using System.Windows.Forms;

namespace App_511_SWAPI
{
    public class PersonDetailsPresenterHw_05
    {
        private readonly Label lblName;
        private readonly Label lblParam1;
        private readonly Label lblParam2;
        private readonly Label lblParam3;

        public PersonDetailsPresenterHw_05(Label nameLabel, Label param1Label, Label param2Label, Label param3Label)
        {
            lblName = nameLabel;
            lblParam1 = param1Label;
            lblParam2 = param2Label;
            lblParam3 = param3Label;
        }

        public void Display(PersonData person)
        {
            if (person == null) return;

            lblName.Text = person.Name;
            lblParam1.Text = $"Height: {person.Height}";
            lblParam2.Text = $"Mass: {person.Mass}";
            lblParam3.Text = $"Birth Year: {person.BirthYear}";
        }
    }
}