using RealEstate;

var Sergey = new Person()
{
    Name = "Сергей",
    Birthday = DateTime.Parse("15.02.1978"),
    Capital = 0,
};
    
var Nikolay = new Person()
{
    Name = "Николай",
    Birthday = DateTime.Parse("01.01.1985"),
    Capital = 40000000,
};

var GoodHouse = new House()
{
    Area = 180,
    Address = "СНТ Подспорье, д.3",
    Owner = Sergey,
};

GoodHouse.PrintSpec();
Sergey.PrintInfo();
Nikolay.PrintInfo();
GoodHouse.Sell(Nikolay, 25000000, true);
GoodHouse.PrintSpec();