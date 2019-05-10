using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace ConanExilesDataTableParser
{
    public partial class formWindow : Form
    {

        List<string> strlistItemIDs = new List<string>();
        List<string> strlistItemNames = new List<string>();

        List<ComboBox> comboboxlistSource = new List<ComboBox>();
        List<ComboBox> comboboxlistChange = new List<ComboBox>();

        public formWindow()
        {
            InitializeComponent();
        }

        private void FormWindow_Shown(object sender, EventArgs e)
        {
        }

        private void FormButtonLoad_Click(object sender, EventArgs e)
        {
            string strPathToItemIDs = Path.GetDirectoryName(Application.ExecutablePath) + "\\Conan Exiles Item IDs.txt";
            string strPathToItemNames = Path.GetDirectoryName(Application.ExecutablePath) + "\\Conan Exiles Item Names.txt";

            if ( File.Exists(strPathToItemIDs) && File.Exists(strPathToItemNames) )
            {
                formTextItemIDs.Text = File.ReadAllText(strPathToItemIDs);
                formTextItemNames.Text = File.ReadAllText(strPathToItemNames);
                formGroupSource.Enabled = true;
                formGroupChange.Enabled = true;
                formButtonBegin.Enabled = true;

                strlistItemIDs = File.ReadAllLines(strPathToItemIDs).ToList();
                strlistItemNames = File.ReadAllLines(strPathToItemNames).ToList();
            }


            for (int iCounter = 0; iCounter < formPanelSource.Controls.Count; iCounter++)
            {


                ComboBox formDropdownSource = formPanelSource.Controls[iCounter] as ComboBox;
                comboboxlistSource.Add(formDropdownSource);

                for (int iCounterList = 0; iCounterList < strlistItemIDs.Count; iCounterList++)
                {
                    formDropdownSource.Items.Add(strlistItemNames[iCounterList]);
                }

                //

                ComboBox formDropdownChange = formPanelChange.Controls[iCounter] as ComboBox;
                comboboxlistChange.Add(formDropdownChange);

                for (int iCounterList = 0; iCounterList < strlistItemIDs.Count; iCounterList++)
                {
                    formDropdownChange.Items.Add(strlistItemNames[iCounterList]);
                }

            }

            //MessageBox.Show($"Number of source | change ComboBoxes: {formPanelSource.Controls.Count} {formPanelChange.Controls.Count}");
            //MessageBox.Show($"Size of source | change Lists: {comboboxlistSource.Count()} {comboboxlistChange.Count()}");

            formButtonLoad.Visible = false;

        }

        public class jsonCERecipe
        {
            public string RowName { get; set; }
            public string RecipeName { get; set; }
            public string ShortDesc { get; set; }
            public string RecipeType { get; set; }
            public int TimeToCraft { get; set; }
            public int Tier { get; set; }
            public int CraftXP { get; set; }
            public int CraftingStations { get; set; }
            public int RequiredFuel { get; set; }
            public string Icon { get; set; }
            public string BuildingModule { get; set; }
            public bool DestroyStationOnComplete { get; set; }
            public int RecipeItemFlags { get; set; }
            public int Ingredient1ID { get; set; }
            public int Ingredient1Quantity { get; set; }
            public int Ingredient2ID { get; set; }
            public int Ingredient2Quantity { get; set; }
            public int Ingredient3ID { get; set; }
            public int Ingredient3Quantity { get; set; }
            public int Ingredient4ID { get; set; }
            public int Ingredient4Quantity { get; set; }
            public int CatalystID { get; set; }
            public int Result1ID { get; set; }
            public int Result1Quantity { get; set; }
            public int Result2ID { get; set; }
            public int Result2Quantity { get; set; }
            public int Result3ID { get; set; }
            public int Result3Quantity { get; set; }
            public int Result4ID { get; set; }
            public int Result4Quantity { get; set; }
            public int Result1Weight { get; set; }
            public int Result2Weight { get; set; }
            public int Result3Weight { get; set; }
            public int Result4Weight { get; set; }
            public int ThrallRecipeFeatRequirement { get; set; }
            public bool IsThrallMachineRecipe { get; set; }
            public bool IsRecipeEnabled { get; set; }
            public string MapMarkerIcon { get; set; }
            public string ExilesJourneyTrigger { get; set; }
            public string DLCPackage { get; set; }
        }

        public void ProcessRecipe()
        {
            List<jsonCERecipe> CERecipeDataTable = new List<jsonCERecipe>();
            CERecipeDataTable = JsonConvert.DeserializeObject<List<jsonCERecipe>>(formSourceText.Text);

            //MessageBox.Show($"Recipe's found: {CERecipeDataTable.Count} - Going through recipe");
            foreach (jsonCERecipe cerecipeCurrent in CERecipeDataTable)
            {

                int iIngredient1 = cerecipeCurrent.Ingredient1ID;
                //MessageBox.Show($"Ingredient 1 is {iIngredient1}");
                int iIngredient2 = cerecipeCurrent.Ingredient2ID;
                //MessageBox.Show($"Ingredient 2 is {iIngredient2}");
                int iIngredient3 = cerecipeCurrent.Ingredient3ID;
                //MessageBox.Show($"Ingredient 3 is {iIngredient3}");
                int iIngredient4 = cerecipeCurrent.Ingredient4ID;
                //MessageBox.Show($"Ingredient 4 is {iIngredient4}");

                //MessageBox.Show($"Number of ComboBox Pairs: {comboboxlistSource.Count()}");

                for (int iCounter = 0; iCounter < (comboboxlistSource.Count() - 1); iCounter++)
                {

                    int iItemEntryPos;

                    ComboBox comboboxCurrentSource = comboboxlistSource[iCounter];
                    ComboBox comboboxCurrentChange = comboboxlistChange[iCounter];

                    string strCurrentSource = comboboxCurrentSource.Text;
                    string strCurrentChange = comboboxCurrentChange.Text;

                    int iSourceIndex = strlistItemNames.FindIndex(strEntry => strEntry == strCurrentSource);
                    //MessageBox.Show($"Source Index is: {iSourceIndex}");
                    int iChangeIndex = strlistItemNames.FindIndex(strEntry => strEntry == strCurrentChange);
                    //MessageBox.Show($"Change Index is: {iSourceIndex}");


                    if (string.IsNullOrEmpty(strCurrentSource) == true || string.IsNullOrEmpty(strCurrentChange) == true || strCurrentSource == "_None")
                    {
                        //MessageBox.Show($"Source is: {strCurrentSource} | Change to: {strCurrentChange} | Skipping");
                        continue;
                    }

                    //MessageBox.Show($"Passed null check: {comboboxCurrentSource.Text} | {comboboxCurrentChange.Text}");
                    iItemEntryPos = strlistItemNames.IndexOf(strCurrentChange);
                    //MessageBox.Show($"{iItemEntryPos}");
                    
                    if (iIngredient1 == Int32.Parse(strlistItemIDs[iSourceIndex]))
                    {
                        //MessageBox.Show($"Turning {strCurrentSource} into {strCurrentChange} for ingredient 1");
                        cerecipeCurrent.Ingredient1ID = Int32.Parse(strlistItemIDs[iItemEntryPos]);
                        //MessageBox.Show($"{cerecipeCurrent.Ingredient1ID}");

                        if (strCurrentChange == "_None") { cerecipeCurrent.Ingredient1Quantity = 0; }

                    }

                    if (iIngredient2 == Int32.Parse(strlistItemIDs[iSourceIndex]))
                    {
                        //MessageBox.Show($"Turning {strCurrentSource} into {strCurrentChange} for ingredient 2");
                        cerecipeCurrent.Ingredient2ID = Int32.Parse(strlistItemIDs[iItemEntryPos]);

                        if (strCurrentChange == "_None") { cerecipeCurrent.Ingredient2Quantity = 0; }
                    }

                    if (iIngredient3 == Int32.Parse(strlistItemIDs[iSourceIndex]))
                    {
                        //MessageBox.Show($"Turning {strCurrentSource} into {strCurrentChange} for ingredient 3");
                        cerecipeCurrent.Ingredient3ID = Int32.Parse(strlistItemIDs[iItemEntryPos]);

                        if (strCurrentChange == "_None") { cerecipeCurrent.Ingredient3Quantity = 0; }
                    }

                    if (iIngredient4 == Int32.Parse(strlistItemIDs[iSourceIndex]))
                    {
                        //MessageBox.Show($"Turning {strCurrentSource} into {strCurrentChange} for ingredient 4");
                        cerecipeCurrent.Ingredient4ID = Int32.Parse(strlistItemIDs[iItemEntryPos]);

                        if (strCurrentChange == "_None") { cerecipeCurrent.Ingredient4Quantity = 0; }
                    }

                }

            }

            formResultText.Enabled = true;
            
            formResultText.Text = JsonConvert.SerializeObject(CERecipeDataTable, Formatting.Indented);
        }
        private void FormButtonBegin_Click(object sender, EventArgs e)
        {
            ProcessRecipe();
            MessageBox.Show("Done");
        }

        private void FormGroup_Enter(object sender, EventArgs e)
        {

        }

        private void FormWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
