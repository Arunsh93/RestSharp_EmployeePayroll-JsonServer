using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace RestSharpTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }


    [TestClass]
    public class RestSharpTestCases
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse getEmployeeList()
        {
            //Arrange
            RestRequest request = new RestRequest("/employees", Method.GET);

            //Action
            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(9, dataResponse.Count);

            foreach(Employee e in dataResponse)
            {
                Console.WriteLine("id: " + e.id + "Name: " + e.name + "Salary: " + e.salary);
            }
        }
        [TestMethod]
        public void givenEmployee_OnPost_shouldReturnAddedEmployee()
        {
            //Arrange
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObject = new JObject();
            jObject.Add("Name", "Muttu");
            jObject.Add("Salary", "36000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);

            //Action
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Muttu", dataResponse.name);
            Assert.AreEqual("36000", dataResponse.salary);
            Console.WriteLine(response.Content);
        }
        [TestMethod]
        public void givenEmployee_OnUpdate_shouldReturnUpdatedEmployee()
        {
            //Arrange
            RestRequest request = new RestRequest("/employees/13", Method.PUT);
            JObject jObject = new JObject();
            jObject.Add("Name", "Uday");
            jObject.Add("Salary", "40000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);

            //Action
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Uday", dataResponse.name);
            Assert.AreEqual("40000", dataResponse.salary);
            Console.WriteLine(response.Content);
        }
        [TestMethod]
        public void givenEmployee_OnDelete_SholudDeleteEmployee()
        {
            RestRequest request = new RestRequest("/employees/7", Method.DELETE);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}
