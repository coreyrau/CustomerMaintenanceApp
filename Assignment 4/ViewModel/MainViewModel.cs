using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight; //For mvvmlight
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;//for class Messenger
using System.Windows.Input;
using System.Windows;
using Assignment_4.Views;
using Assignment_4.Messages;
using Assignment_4;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Assignment_4.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        private int custID;
        private string names;
        private string address;
        private string city;
        private string state;
        private string zipcode;

        // Values are being assigned to each value above everysinge time the getcustomer button is pressed
        // but for some reason it is not updating
        // the next step is to make the properties binded to the textboxes update in the window

        private Customer selectedCustomer = new Customer();
        public ICommand GetCustomerCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand ModifyCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public RelayCommand<IClosable> ExitCommand { get; private set; }

        public MainViewModel()
        {
            GetCustomerCommand = new RelayCommand(GetCustomerMethod);
            AddCommand = new RelayCommand(AddMethod);
            ModifyCommand = new RelayCommand(ModifyMethod);
            DeleteCommand = new RelayCommand(DeleteMethod);
            this.ExitCommand = new RelayCommand<IClosable>(this.CloseWindow);
            Messenger.Default.Register<MessageMember>(this, ModifyReciever);
        }

        public void ModifyReciever(MessageMember member)
        {
            selectedCustomer = member.Customer;
            if (member.Message == "Modify")
            {
                Names = selectedCustomer.Name;
                Address = selectedCustomer.Address;
                City = selectedCustomer.City;
                State = selectedCustomer.State;
                Zip = selectedCustomer.ZipCode;
                this.RaisePropertyChanged();
                MessageBox.Show("Member Updated");
            }
            else if (member.Message == "Add")
            {
                CustID = selectedCustomer.CustomerID;
                Names = selectedCustomer.Name;
                Address = selectedCustomer.Address;
                City = selectedCustomer.City;
                State = selectedCustomer.State;
                Zip = selectedCustomer.ZipCode;
                this.RaisePropertyChanged();
                MessageBox.Show("Added Member");
            }
        }

        public void GetCustomerMethod()
        {
            try
            {
                Customer getCustomer =
                    (from c in MMABooksClass.context.Customers
                     where c.CustomerID == custID
                     select c).Single();

                selectedCustomer = getCustomer;

                Names = selectedCustomer.Name;
                Address = selectedCustomer.Address;
                City = selectedCustomer.City;
                State = selectedCustomer.State;
                Zip = selectedCustomer.ZipCode;

                if (selectedCustomer == null)
                {
                    MessageBox.Show("Error");
                    Names = "";
                    Address = "";
                    City = "";
                    State = "";
                    Zip = "";
                }
                else
                {
                    if (!MMABooksClass.context.Entry(selectedCustomer).Reference("State1").IsLoaded)
                    {
                        MMABooksClass.context.Entry(selectedCustomer).Reference("State1").Load();
                    }
                }

                MessageBox.Show("Customer ID: " + selectedCustomer.CustomerID + "\n" + "Name: " + selectedCustomer.Name + "\n" + "Address: "
                 + selectedCustomer.Address + "\n" + "City: " + selectedCustomer.City + "\n" + "State: " + selectedCustomer.State1.StateName + "\n" + "Zip: "
                 + selectedCustomer.ZipCode + "\n", "Values To Be Added");

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Null reference", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show("Null reference", "Error");
            }

        }

        public void AddMethod()
        {
            AddView add = new AddView();
            add.Show();
        }

        public void ModifyMethod()
        {
            if (selectedCustomer != null)
            {
                ModifyView change = new ModifyView();
                Messenger.Default.Send(new MessageMember(selectedCustomer, "edit"));
                change.Show();
            }
        }

        public void DeleteMethod()
        {
            MessageBox.Show(("Delete: " + Names + "\n" + Address), "Delete");
            try
            {
                MMABooksClass.context.Customers.Remove(selectedCustomer);
                MMABooksClass.context.SaveChanges();
                Messenger.Default.Send(new NotificationMessage("Customer Removed!"));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                if (MMABooksClass.context.Entry(selectedCustomer).State == EntityState.Detached)
                {
                    MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error");
                }
                else
                {
                    MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error");
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Null reference", "Error");
            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Error");
            }
            ClearControls();
        }

        private void CloseWindow(IClosable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public int CustID
        {
            get
            {
                return custID;
            }
            set
            {
                this.custID = value;
                this.RaisePropertyChanged();
            }
        }

        public string Names
        {
            get
            {
                return names;
            }
            set
            {
                this.names = value;
                this.RaisePropertyChanged();
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                this.address = value;
                this.RaisePropertyChanged();
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                this.city = value;
                this.RaisePropertyChanged();
            }
        }

        public string State
        {
            get { return state; }
            set
            {
                this.state = value;
                this.RaisePropertyChanged();
            }
        }

        public string Zip
        {
            get { return zipcode; }
            set
            {
                this.zipcode = value;
                this.RaisePropertyChanged();
            }
        }

        private void ClearControls()
        {
            CustID = 0;
            Names = "";
            Address = "";
            City = "";
            State = "";
            Zip = "";
        }

    }
}