using System;
using System.Collections;
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
        public void ShouldFindInExampleClass()
        {
            DateTimeChecker dateTimeChecker = new DateTimeChecker();

            dateTimeChecker.IsItInHere(typeof(ClassWithDateTime));

        }





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
        public void ShouldFindDateTime_In_ClassWithDateTime()
        {


            //Arrange
            DateTimeChecker dateTimeChecker = new DateTimeChecker();
            Type classType = typeof(ClassWithDateTime);
            string methodToTest = "MethodWithDateTime";

            String methodName =
                classType.GetMethods()
                .Single(w => w.Name.Contains(methodToTest)).Name;

            //Act
            IEnumerable<string> methodNames = dateTimeChecker.MethodsContainingDateTimeInAssembly(classType);



            //Assert
            Assert.That(methodNames.Where(w => w == methodName), Is.Not.Empty);

        }
        [Test]
        public void ShouldFindDateTime_In_PrivateClassWithDateTime()
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
