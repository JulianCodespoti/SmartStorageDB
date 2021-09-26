using FurnitureInventorySystem.Controllers;
using FurnitureInventorySystem.StoreObjects;
using FurnitureInventorySystem.Validation;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FurnitureInventorySystem.View
{
    public partial class SmartForm : Form
    {
        // To control the retrieval and editing of factories and furniture.
        private readonly FactoryController _factoryController;
        private readonly FurnitureController _furnitureController;

        public SmartForm()
        {
            InitializeComponent();
            _factoryController = new FactoryController();
            _furnitureController = new FurnitureController();
        }

        private void SmartForm_Load(object sender, EventArgs e)
        {
            // Add the factories to the factory drop down menu.
            var factoriesToAdd = _factoryController.GetAllFactories();
            foreach (var factory in factoriesToAdd)
            {
                SelectFactory.Items.Add(factory.Name);
            }
        }

        // WHEN CLICKING THE "ADD FURNITURE" BUTTON.
        private void AddButton_Click_1(object sender, EventArgs e)
        {
            // If the name is not valid, return immediately.
            if (!ValidateFurnitureAdd()) return;

            // Adds Furniture to the Selected Factory's list of furniture through custom inputs.
            var furnitureToAdd = new Furniture
            {
                // Constructs Furniture to add with the inputs from the text box.
                Factory_Id = _factoryController.SelectedFactory.Id,
                Name = AddNameBox.Text,
                Quantity = int.Parse(AddQuantityBox.Text),
                Price = decimal.Parse(AddPriceBox.Text)
            };
            _furnitureController.AddFurniture(furnitureToAdd);
            // Updates UI.
            UpdateDatabaseDisplay();
        }

        // WHEN CLICKING THE "REMOVE FURNITURE" BUTTON.
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateFurnitureRemove()) return;
            // Removes from the quantity of a specified Furniture. If the quantity to remove exceeds the quantity
            // of said furniture, then remove that Furniture.
            var selectedFurnitureName = RemoveFurnitureNameComboBox.SelectedItem.ToString();
            var selectedFurniture = _furnitureController.GetByName(selectedFurnitureName, _factoryController.SelectedFactory.Id);
            var selectedQuantity = RemoveFurnitureQuantityComboBox.SelectedIndex;

            // If quantity to remove is equal to the remaining quantity of the furniture, remove it.
            if (selectedQuantity == selectedFurniture.Quantity)
            {
                _furnitureController.RemoveFurniture(selectedFurniture.Id);
                RemoveFurnitureNameComboBox.Items.Remove(selectedFurniture.Name);
                UpdateFurnitureNameComboBox.Items.Remove(selectedFurniture.Name);
            }
            // Otherwise update the furniture's quantity.
            else
            {
                _furnitureController.RemoveQuantity(selectedFurniture, selectedQuantity);
            }
            // When selecting a furniture, clear the quantity box and replace it with the quantity of the specified furniture
            RemoveFurnitureQuantityComboBox.Items.Clear();
            for (int i = 0; i <= selectedFurniture.Quantity; i++)
            {
                RemoveFurnitureQuantityComboBox.Items.Add(i);
            }
            // Updates UI.
            UpdateDatabaseDisplay();
        }

        // WHEN CLICKING THE "UPDATE FURNITURE" BUTTON.
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            // If the name is not valid, return immediately.
            if (!ValidateFurnitureUpdate()) return;

            // Retrieves the selected furniture to update from the combobox.
            var selectedFurnitureName = UpdateFurnitureNameComboBox.SelectedItem.ToString();
            var selectedFurniture =
                _furnitureController.GetByName(selectedFurnitureName, _factoryController.SelectedFactory.Id);

            // Creates a furniture object from the constructor.
            var furnitureToUpdate = new Furniture
            {
                // Constructs Furniture to add with the inputs from the text boxes.
                Factory_Id = _factoryController.SelectedFactory.Id,
                Name = NewNameTextBox.Text,
                Quantity = int.Parse(NewQuantityTextBox.Text),
                Price = decimal.Parse(NewPriceTextBox.Text),
                Id = selectedFurniture.Id
            };
            // Update the furniture with the supplied information.
            _furnitureController.UpdateFurniture(furnitureToUpdate, _factoryController.SelectedFactory.Id);
            //Update
            UpdateDatabaseDisplay();
        }

        private void SelectFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When selecting a factory, erase all error messages currently displayed on the screen.
            EraseErrorMessages();

            // Loads the information regarding a specific factory when selected in drop down menu.
            var selectedFactoryName = SelectFactory.SelectedItem.ToString();
            _factoryController.SelectedFactory = _factoryController.FindByName(selectedFactoryName);

            // Displays the factory's email and address.
            factoryOutput.Text = _factoryController.SelectedFactory.Address;
            factoryEmail.Text = _factoryController.SelectedFactory.Email;

            // Retrieves database information as a DataTable and sets it to the UI's datatable's datasource.
            FurnitureDataTable.DataSource = _furnitureController.LoadFurnitureAsDataTableFrom(_factoryController.SelectedFactory.Id);
            DisplayUIPanel.Visible = true;

            // Loads the information regarding said factory in the 'remove furniture' drop down menu.
            UpdateDatabaseDisplay();
        }

        private void RemoveFurnitureNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When selecting a factory name from the 'Remove Furniture' drop down menu, add its corresponding
            // quantity to it's quantity drop down menu.
            var selectedFurnitureName = RemoveFurnitureNameComboBox.SelectedItem.ToString();
            var selectedFurniture = _furnitureController.GetByName(selectedFurnitureName, _factoryController.SelectedFactory.Id);

            RemoveFurnitureQuantityComboBox.Items.Clear();
            for (int i = 0; i <= selectedFurniture.Quantity; i++)
            {
                RemoveFurnitureQuantityComboBox.Items.Add(i);
            }
        }

        private void UpdateFurnitureNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When selecting a factory name from the 'Update Furniture' drop down menu, add its price and quantity
            // to their corresponding text boxes.
            var selectedFurnitureName = UpdateFurnitureNameComboBox.SelectedItem.ToString();
            var selectedFurniture = _furnitureController.GetByName(selectedFurnitureName, _factoryController.SelectedFactory.Id);

            CurrentPriceTextBox.Text = selectedFurniture.Price.ToString();
            CurrentQuantityTextBox.Text = selectedFurniture.Quantity.ToString();
        }



        private void UpdateDatabaseDisplay()
        {   // Takes values from the database file and adjusts the main DataTable, as well as comboboxes
            // with the updated values after a change has occurred.
            FurnitureDataTable.DataSource = _furnitureController.LoadFurnitureAsDataTableFrom(_factoryController.SelectedFactory.Id);
            var furnitureToAdd = _furnitureController.GetFurnitureFrom(_factoryController.SelectedFactory.Id);
            RemoveFurnitureNameComboBox.Items.Clear();
            UpdateFurnitureNameComboBox.Items.Clear();
            RemoveFurnitureQuantityComboBox.Items.Clear();
            foreach (var furniture in furnitureToAdd)
            {
                RemoveFurnitureNameComboBox.Items.Add(furniture.Name);
                UpdateFurnitureNameComboBox.Items.Add((furniture.Name));
            }
            CurrentPriceTextBox.Clear();
            CurrentQuantityTextBox.Clear();
            NewNameTextBox.Clear();
            NewPriceTextBox.Clear();
            NewQuantityTextBox.Clear();
            AddNameBox.Clear();
            AddPriceBox.Clear();
            AddQuantityBox.Clear();
            
        }

        private bool ValidateFurnitureAdd()
        {
            //Validates the information provided to the textboxes when clicking the "Add Furniture" button.
            EraseErrorMessages();
            var furnitureToAdd = _furnitureController.GetFurnitureFrom(_factoryController.SelectedFactory.Id);
            var validationCheck = true;
            var nameIsUnique = true;
            foreach (Furniture furniture in furnitureToAdd)
            {
                if (furniture.Name == AddNameBox.Text)
                {
                    InvalidName.Text = "Please enter a UNIQUE name.";
                    InvalidName.Visible = true;
                    validationCheck = false;
                    nameIsUnique = false;
                }
            }
            if (!InputValidator.ValidString(AddNameBox.Text) && nameIsUnique)
            {
                InvalidName.Visible = true;
                validationCheck = false;
            }
            if (!InputValidator.ValidInteger(AddQuantityBox.Text))
            {
                InvalidQuantity.Visible = true;
                validationCheck = false;
            }
            if (!InputValidator.ValidDecimal(AddPriceBox.Text))
            {
                InvalidPrice.Visible = true;
                validationCheck = false;
            }

            return validationCheck;
        }

        private bool ValidateFurnitureRemove()
        {
            //Validates the information provided to the textboxes when clicking the "Remove Furniture" button.
            EraseErrorMessages();

            if (RemoveFurnitureNameComboBox.SelectedItem == null)
            {
                InvalidNameRemoveSelect.Visible = true;
                return false;
            }

            if (RemoveFurnitureQuantityComboBox.SelectedItem == null)
            {
                UnselectedQuantity.Visible = true;
                return false;
            }

            return true;
        }

        private bool ValidateFurnitureUpdate()
        {
            //Validates the information provided to the textboxes when clicking the "Update Furniture" button.
            EraseErrorMessages();
            bool validationCheck = true;
            if (UpdateFurnitureNameComboBox.SelectedItem == null)
            {
                InvalidUpdateFurnitureSelect.Visible = true;
                return false;
            }
            if (!InputValidator.ValidString(NewNameTextBox.Text))
            {
                InvalidUpdateName.Visible = true;
                validationCheck = false;
            }
            if (!InputValidator.ValidInteger(NewQuantityTextBox.Text))
            {
                InvalidUpdateQuantity.Visible = true;
                validationCheck = false;
            }
            if (!InputValidator.ValidDecimal(NewPriceTextBox.Text))
            {
                InvalidUpdatePrice.Visible = true;
                validationCheck = false;
            }
            return validationCheck;
        }

        public void EraseErrorMessages()
        {
            //Erases all error messages on the screen.
            InvalidNameRemoveSelect.Visible = false;
            UnselectedQuantity.Visible = false;
            InvalidUpdateName.Visible = false;
            InvalidUpdateQuantity.Visible = false;
            InvalidUpdatePrice.Visible = false;
            InvalidName.Visible = false;
            InvalidQuantity.Visible = false;
            InvalidPrice.Visible = false;
            InvalidUpdateFurnitureSelect.Visible = false;
            // When a provided name is no longer unique (is already in the database), instead, display this.
            InvalidName.Text = "Please enter a valid name.";
        }

        private void DisplayUIPanel_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}