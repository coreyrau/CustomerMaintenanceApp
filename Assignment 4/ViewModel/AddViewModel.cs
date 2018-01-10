using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight; //For mvvmlight
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;//for class Messenger
using System.Windows.Input;
using System.Collections.ObjectModel;
using Assignment_4.Messages;
using System.Windows;
using System.Data.Entity.Infrastructure;
using System.Data;
using Assignment_4.ViewModel;
using System.Data.Entity;

namespace Assignment_4.ViewModel
{
    public class AddViewModel : ViewModelBase
    {
        private ObservableCollection<State> states = new ObservableCollection<State>();
        private State selectedstate = new State();
        public RelayCommand<IClosable> AcceptCommand { get; private set; }
        public RelayCommand<IClosable> CancelCommand { get; private set; }
        private string name;
        private string address;
        private string city;
        private string zipcode;

        public AddViewModel()
        {
            AcceptCommand = new RelayCommand<IClosable>(this.AcceptMethod);
            CancelCommand = new RelayCommand<IClosable>(this.CancelMethod);

            var stateslist =
                (from state in MMABooksClass.context.States orderby state.StateName select state).ToList();
            states = new ObservableCollection<State>(stateslist);
        }

        public void AcceptMethod(IClosable window)
        {
            if (Name != null && City != null && Address != null && SelectedState != null && Zipcode != null)
            {
                try
                {
                    var state = (from cust in MMABooksClass.context.Customers
                                 where cust.State1.StateName == SelectedState.StateName
                                 select cust.State).FirstOrDefault();

                    Customer customer = new Customer();
                    customer.Name = Name;
                    customer.Address = Address;
                    customer.City = City;
                    customer.State = state;
                    customer.ZipCode = Zipcode;

                    MMABooksClass.context.Customers.Add(customer);
                    MMABooksClass.context.SaveChanges();
                    MessageBox.Show(customer.CustomerID.ToString(), "Customer Added ID");
                    Messenger.Default.Send(new MessageMember(customer, "Add"));

                    Name = "";
                    Address = "";
                    City = "";
                    SelectedState = null;
                    Zipcode = "";

                    if (window != null)
                    {
                        window.Close();
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    if (MMABooksClass.context.Entry(selectedstate).State == EntityState.Detached)
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
            }
            else
            {
                MessageBox.Show("ERROR: NULL DATA");
            }
        }

        public void CancelMethod(IClosable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }

        public ObservableCollection<State> States
        {
            get
            {
                return states;
            }
            set
            {
                states = value;
            }
        }

        public State SelectedState
        {
            get
            {
                return selectedstate;

            }
            set
            {
                selectedstate = value;
            }
        }

        public string Zipcode
        {
            get
            {
                return zipcode;
            }
            set
            {
                zipcode = value;
            }
        }
    }

    public interface IClosable
    {
        void Close();
    }
}
