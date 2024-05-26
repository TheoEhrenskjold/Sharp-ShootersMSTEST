
using Sharp_Shooters;
using System.Diagnostics;
using System.Xml.Linq;

namespace TESTMS
{
    [TestClass]
    public class UnitTest1
    {
        private User _user;
        private List<Accounts> _accounts;
        

        [TestMethod] //Testing USD to USD
        public void ConvertCurrency_SameCurrency_ReturnsSame()
        {
            var fromAccount = new Accounts("Test", 100, "USD", "$");
            var toAccount = new Accounts("Test", 0, "USD", "$");

            var actual = Currency.ConvertCurrency(100, fromAccount, toAccount);

            Assert.AreEqual(100, actual);
        }

        [TestMethod] //Testing USD to Euro conversion
        public void ConvertCurrency_USDtoEUR_Correct()
        {
            var fromAccount = new Accounts("Test", 100, "USD", "$");
            var toAccount = new Accounts("Test", 0, "EURO", "E");

            var actual = Currency.ConvertCurrency(100, fromAccount, toAccount);

            Assert.AreEqual(93, actual);
        }

        [TestMethod] //Testing USD to an invalid currency.
        public void ConvertCurrency_USDtoInvalid_Exception()
        {
            var fromAccount = new Accounts("Test", 100, "USD", "$");
            var toAccount = new Accounts("Test", 0, "Dinar", "DN");

            var actual = Currency.ConvertCurrency(100, fromAccount, toAccount);

            Assert.AreEqual(100, actual);
        }

        [TestMethod] //Testing null to invalid currency
        public void ConvertCurrency_NulltoDinar_0Transaction_Returns0()
        {
            var fromAccount = new Accounts("Test", 100, null, "$");
            var toAccount = new Accounts("Test", 0, "Dinar", "DN");

            var actual = Currency.ConvertCurrency(100, fromAccount, toAccount);

            Assert.AreEqual(100, actual);
        }

        

        [TestMethod] //Testing the total loanamount
        public void CalculateTotalBorrowedAmount_CorrectlyCalculatesTotal()
        {
            var loanTransactions = new List<LoanTransaction>
            {
              new LoanTransaction(100),
              new LoanTransaction(200),
              new LoanTransaction(300)
            };

            Assert.AreEqual(600, Currency.CalculateTotalBorrowedAmount(loanTransactions));
        }


        [TestMethod] //Testing if the base value is null. The method throws an exception
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateTotalBorrowedAmount_NullList_ThrowsArgumentNullException()
        {
            Currency.CalculateTotalBorrowedAmount(null);
        }

        [TestMethod] //Testing if the account has negativ balance
        public void CalculateTotalBorrowedAmount_NegativeBlance_NegativBorrowAmount()
        {
            var loanTransactions = new List<LoanTransaction>
            {
              new LoanTransaction(-100)

            };
            Assert.AreEqual(-100, Currency.CalculateTotalBorrowedAmount(loanTransactions));
        }
        [TestMethod] //Testing if the method adds the correct amount of users
        public void InitializeUser_ReturnsCorrectNumberOfUsers()
        {
            var users = Admin.InitializeUser();

            //Kontrollera att rätt antal användare har skapats
            Assert.AreEqual(4, users.Count);
        }

        [TestClass] //Testing if Theo is in the list, Theos pincode is 1111, Theo has 3 accounts. Fail the test with the accountbalance.
        public class UserInitializationTests
        {
            [TestMethod]
            public void InitializeUser_TheoHasIncorrectBalance_ShouldFail()
            {
                var users = Admin.InitializeUser();
                var theo = users.FirstOrDefault(u => u.UserName == "theo");

                Assert.IsNotNull(theo);
                Assert.AreEqual(1111, theo.PinCode);
                Assert.AreEqual(3, theo.Accounts.Count);

                //Förväntar sig ett felaktigt saldo, vilket kommer att leda till att testet misslyckas
                Assert.AreEqual(9999, theo.Accounts[0].AccountBalance); // Felaktigt värde
            }
        }
    }
}