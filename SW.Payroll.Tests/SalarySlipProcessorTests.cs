using Moq;
using System;
using System.Reflection;
using System.Text;
using Xunit;
using static System.Collections.Specialized.BitVector32;

namespace SW.Payroll.Tests
{
    public class SalarySlipProcessorTests
    {
        //how to name a testing method:
        //[Fact]
        //public void MethodName_Scenario_Outcome()
        //{
        //
        //}

        //The purpose of the[Fact] attribute in xUnit.net is to mark a method as a test case that should be executed by the test runner.
        [Fact]
        public void CalculateBasicSalary_ForEmployeeWageAndWorkingDays_ReturnsBasicSalary()
        {
            //Arrange : in this step we initialize obj we will test the class's method with
            var employee = new Employee() { Wage = 500, WorkingDays = 20 };

            //Act : in this step we call the method to manipulate

            var SalarySlipProcessor = new SalarySlipProcessor(null);

            var actual = SalarySlipProcessor.CalculateBasicSalary(employee);
            var Expected = 10000m;


            //Assert : Assert is used in unit testing to verify that expected conditions are met.
            //If an assertion fails, the test fails.

            Assert.Equal(Expected, actual);
        }

        [Fact]
        public void CalculateBasicSalary_ForEmployeeIsNull_ThrowArgumentNullException()
        {
            //Arrange : 
            Employee employee = null;

            //Act :
            var SalarySlipProcessor = new SalarySlipProcessor(null);

            //we use delegate here to not let CalculateBasicSalary() executes and throw excpetion and test fails
            Func<Employee,decimal> Func =  (Empl) => SalarySlipProcessor.CalculateBasicSalary(Empl);

            //Assert :

            //Assert.Throws<T>(Action) This method checks whether the provided action throws an exception of type T.
                  Assert.Throws<ArgumentNullException>(() => Func(employee));
        }

        [Fact]
        public void CalculateTransportationAllowece_ForEmployeeIsNull_ThrowAgumentNullException()
        {
            //Arrange
            Employee employee = null;

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee,decimal> Func = (empl) => salarySlipProcessor.CalculateTransportationAllowece(empl);

            //assert

            Assert.Throws<ArgumentNullException>(() => Func(employee));
        }

        [Fact]
        public void CalculateTransportationAllowece_ForEmployeeWorkInOffice_ReturnTransportationAllowanceAmount()
        {
            //arrange

            Employee employee = new Employee() { WorkPlatform = WorkPlatform.Office };

            //act 
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var Expected = Constants.TransportationAllowanceAmount;

            //assert

            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculateTransportationAllowece_ForEmployeeWorkRemote_ReturnTransportationAllowanceAmount()
        {
            //arrange

            Employee employee = new Employee() { WorkPlatform = WorkPlatform.Remote };

            //act 
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var Expected = 0m;

            //assert

            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculateTransportationAllowece_ForEmployeeWorkHybrid_ReturnTransportationAllowanceAmount()
        {
            //arrange
            Employee employee = new Employee() { WorkPlatform = WorkPlatform.Hybrid };

            //act 
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateTransportationAllowece(employee);
            var Expected = Constants.TransportationAllowanceAmount / 2;

            //assert
            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculateDangerPay_ForEmployeeIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Employee employee = null;

            //act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);

            Func<Employee, decimal> Func = (emp) => salarySlipProcessor.CalculateDangerPay(emp);

            //Assert

            Assert.Throws<ArgumentNullException>(() => Func(employee));

        }
        [Fact]
        public void CalculateDangerPay_EmployeeIsDangerOn_ReturnDangerPayAmount()
        {
            //arrange
            Employee employee = new Employee() { IsDanger = true };

            //act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);

            var Actual   = salarySlipProcessor.CalculateDangerPay(employee);
            var Expected = Constants.DangerPayAmount;

            //Assert

            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculateDangerPay_EmployeeIsDangerOffAndInDangerZone_ReturnDangerPayAmount()
        {
            //arrange
            Employee employee = new Employee() { IsDanger = false,DutyStation = "Gaza" };
            var mock = new Mock<IZoneService>();
            var setup = mock.Setup(z => z.IsDangerZone(employee.DutyStation)).Returns(true);
          
            //act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(mock.Object);

            var Actual = salarySlipProcessor.CalculateDangerPay(employee);
            var Expected = Constants.DangerPayAmount;

            //Assert

            Assert.Equal(Expected, Actual);
        }
        [Fact]
        public void CalculateDangerPay_EmployeeInDangerOffAndNotInDangerZone_ReturnDangerPayAmount()
        {
            //arrange
            Employee employee = new Employee() { IsDanger = false, DutyStation = "Gaza" };
            var mock = new Mock<IZoneService>();
            var setup = mock.Setup(z => z.IsDangerZone(employee.DutyStation)).Returns(false);

            //act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(mock.Object);

            var Actual = salarySlipProcessor.CalculateDangerPay(employee);
            var Expected = 0m;

            //Assert

            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculateSpouseAllowance_ForEmployeeIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            Employee employee = null;

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (Empl) => salarySlipProcessor.CalculateSpouseAllowance(Empl);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }
        [Fact]
        public void CalculateSpouseAllowance_EmployeeIsMariedTrue_returnSpouseAllowanceAmount()
        {
            //Arrange
            Employee employee = new Employee() { IsMarried = true };

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateSpouseAllowance(employee);
            var Expected = Constants.SpouseAllowanceAmount;

            //assert

            Assert.Equal(Expected, Actual);
        }
        [Fact]
        public void CalculateSpouseAllowance_EmployeeIsMariedFalse_returnZeroAmout()
        {
            //Arrange
            Employee employee = new Employee() { IsMarried = false };

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateSpouseAllowance(employee);
            var Expected = 0m;

            //assert

            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculateDependancyAllowance_ForEmployeeIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            Employee employee = null;

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (Empl) => salarySlipProcessor.CalculateDependancyAllowance(Empl);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }

        [Fact]
        public void CalculateDependancyAllowance_ForEmployeeTotalDependenciesLessThat0_ThrowsArgumentNullException()
        {
            //Arrange
            Employee employee = new Employee() { TotalDependancies = -1};

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (Empl) => salarySlipProcessor.CalculateDependancyAllowance(Empl);

            //Assert

            Assert.Throws<ArgumentOutOfRangeException>(() => func(employee));
        }

        [Fact]
        public void CalculateDependancyAllowance_ForEmployeeTotalDependenciesMoreThanMax_ReturnMaxDependancyAllowanceAmount()
        {
            //MaxDependantsFactor = 5;

            //Arrange
            Employee employee = new Employee() { TotalDependancies =  6};

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateDependancyAllowance(employee);
            var Expected = Constants.MaxDependancyAllowanceAmount; //200m

            //Assert

            Assert.Equal(Expected,Actual);
        }

        [Fact]
        public void CalculateDependancyAllowance_ForEmployeeTotalDependenciesIs0_RetunZeroAmount()
        {
            //Arrange
            Employee employee = new Employee() { TotalDependancies = 0 };

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateDependancyAllowance(employee);
            var Expected = 0m;

            //Assert

            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculateDependancyAllowance_WhenWithinRange_RetunsCorrectAmount()
        {
            //Arrange
            Employee employee = new Employee() { TotalDependancies = 4 };

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculateDependancyAllowance(employee);
            var Expected = 4 * Constants.DependancyAllowancePerChildAmount;

            //Assert

            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void CalculatePention_EmployeeIsNull_ThrowsArgumentNullException()
        {
            //Arrange
            Employee employee = null;

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            Func<Employee, decimal> func = (Empl) => salarySlipProcessor.CalculatePension(Empl);

            //Assert

            Assert.Throws<ArgumentNullException>(() => func(employee));
        }

        [Fact]
        public void CalculatePension_EmployeeWithoutPensionPlan_ReturnZero()
        {
            //Arrange
            Employee employee = new Employee() { HasPensionPlan = false };

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculatePension(employee);
            var Expected = 0m;

            //Assert

            Assert.Equal(Expected, Actual);
        }
        [Fact]
        public void CalculatePension_EmployeeWitPensionPlan_ReturnsCorrectAmount()
        {
            //Arrange
            Employee employee = new Employee() {Wage = 100, HasPensionPlan = true,WorkingDays = 10};

            //Act
            SalarySlipProcessor salarySlipProcessor = new SalarySlipProcessor(null);
            var Actual = salarySlipProcessor.CalculatePension(employee);
            var Expected = Constants.PensionRate * (100 * 10);

            //Assert

            Assert.Equal(Expected, Actual);
        }
      
        public decimal CalculateTax(Employee employee)
        {
            // var basicSalary = CalculateBasicSalary(employee);
            //   if (basicSalary >= Constants.MediumSalaryThreshold)
            //   return basicSalary * Constants.HighSalaryTaxFactor;
            //else if (basicSalary >= Constants.LowSalaryThreshold)
            //     return basicSalary * Constants.MediumSalaryTaxFactor;
            // else
            // return basicSalary * Constants.LowSalaryTaxFactor;
            return 0m;
        }
    }
}
