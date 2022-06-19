namespace RealEstate
{ 
    public class Person
    {
        public string? Name { get; set; }
        public DateTime Birthday { get; set; }

        public long Capital { get; set; }
        public int Age
        {
            get
            {
                var endDate = DateTime.Now;
                int years = endDate.Year - Birthday.Year;

                if ((Birthday.Month == endDate.Month && endDate.Day < Birthday.Day)
                    || endDate.Month < Birthday.Month)
                    years--;

                return years;
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine("{0} хороший парень. Его возраст {1}. Его капитал {2}", Name, Age, Capital);
        }
    }

    class House
    {
        public int? Area { get; set; }

        public string? Address { get; set; }

        public Person? Owner { get; set; }

        public void Sell(Person NewOwner, long price, bool ShowLogs)
        {
            if (ShowLogs)
            {
                Console.WriteLine("\n------------ внимание, продажа дома --------------\n");
                Console.WriteLine("Дом по адресу {0} продается", Address);
            }

            Owner.Capital += price;

            if (ShowLogs)
                Console.WriteLine("{0} разбогател. Его капитал теперь {1}", Owner.Name, Owner.Capital);
            
            Owner = NewOwner;
            NewOwner.Capital -= price;

            if (ShowLogs)
            {
                Console.WriteLine("{0} стал беднее. Его капитал теперь {1}", Owner.Name, Owner.Capital);
                Console.WriteLine("\n------------ продажа дома закончена --------------\n");
            }
        }

        public void PrintSpec()
        {
            Console.WriteLine(
                "Домом по адресу {0} пллощадью {1} владеет {2}",
                Address,
                Area,
                Owner.Name,
                Owner.Age,
                Owner.Capital);
        }
    }
}
