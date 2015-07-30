using System;
using System.Collections.Generic;
using System.Linq;
using DateTimeCheck;
using ExampleClass;
using NUnit.Framework;


namespace UnitTestProject
{
    [TestFixture]
    public class UnitTest
    {
      
        [Test]
        public void ShouldNOTFindDateTime_In_ClassWithoutDateTime()
        {
            //Arrange
            DateTimeChecker dateTimeChecker = new DateTimeChecker();
            Type classType = typeof (ClassWithoutDateTime);
            string methodToTest = "MethodWithoutDateTime";

            String methodName =
                classType.GetMethods()
                .Single(w => w.Name.Contains(methodToTest)).Name;

            //Act
            IEnumerable<string> methodNames = dateTimeChecker.MethodsContainingDateTimeInAssembly(classType);

            //Assert
            Assert.That(methodNames.Where(w => w == methodName), Is.Empty);

        }


        [Test]
        public void ShouldFindDateTime_In_ClassWithDateTime_Now()
        {
            //Arrange
            DateTimeChecker dateTimeChecker = new DateTimeChecker();
            Type classType = typeof(ClassWithDateTime);
            string methodToTest = "MethodWithDateTimeNow";

            String methodName =
                classType.GetMethods()
                .Single(w => w.Name.Contains(methodToTest)).Name;

            //Act
            IEnumerable<string> methodNames = dateTimeChecker.MethodsContainingDateTimeInAssembly(classType);

            //Assert
            Assert.That(methodNames.Where(w => w == methodName), Is.Not.Empty);
        }


        [Test]
        public void ShouldFindDateTime_In_ClassWithDateTime_UtcNow()
        {
            //Arrange
            DateTimeChecker dateTimeChecker = new DateTimeChecker();
            Type classType = typeof(PrivateClassWrapper);
            string methodToTest = "MethodWithDateTimeToday";

            //Act
            IEnumerable<string> methodNames = dateTimeChecker.MethodsContainingDateTimeInAssembly(classType);

            //Assert
            Assert.That(methodNames.Where(w => w == methodToTest), Is.Not.Empty);
        }


        [Test]
        public void ShouldFindDateTime_In_ClassWithDateTime_Today()
        {
            //Arrange
            DateTimeChecker dateTimeChecker = new DateTimeChecker();
            Type classType = typeof(PrivateClassWrapper);
            string methodToTest = "MethodWithDateTimeUtcNow";

            //Act
            IEnumerable<string> methodNames = dateTimeChecker.MethodsContainingDateTimeInAssembly(classType);

            //Assert
            Assert.That(methodNames.Where(w => w == methodToTest), Is.Not.Empty);
        }


        [Test]
        public void ShouldFindDateTime_In_PrivateClassWithDateTimeNow()
        {
            //Arrange
            DateTimeChecker dateTimeChecker = new DateTimeChecker();
            Type classType = typeof(PrivateClassWrapper);
            string methodToTest = "PrivateMethod";

            //Act
            IEnumerable<string> methodNames = dateTimeChecker.MethodsContainingDateTimeInAssembly(classType);

            //Assert
            Assert.That(methodNames.Where(w => w == methodToTest), Is.Not.Empty);
        }
    }
}
