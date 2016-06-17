using System.Net.Http;
using NUnit.Framework;

namespace MutationTestingExercises.Exercise2WebApi
{
    [TestFixture]
    public class RequestHandlerResolverShould
    {
        private const string KITTENS_ROOT = "kittens/";
        private const string KITTENS_RETRIEVE = "kittens/{0}";
        private RequestHandlerResolver _requestHandlerResolver;

        [SetUp]
        public void SetUp()
        {
            _requestHandlerResolver = new RequestHandlerResolver();
        }

        [Test]
        public void Resolve_a_create_kitten_request()
        {
            var requestHandler = _requestHandlerResolver.Resolve(HttpMethod.Post, KITTENS_ROOT);

            Assert.That(requestHandler, Is.TypeOf<CreateKittenRequestHandler>());
        }

        [TestCase("Dog")]
        [TestCase("Bob")]
        [TestCase("fluffles")]
        public void Resolve_a_retrieve_kitten_request(string kittenName)
        {
            string kittenUrl = string.Format(KITTENS_RETRIEVE, kittenName);
            var requestHandler = _requestHandlerResolver.Resolve(HttpMethod.Get, kittenUrl);

            Assert.That(requestHandler, Is.TypeOf<RetrieveKittenRequestHandler>());
        }

        [TestCaseSource("UnrecognizedRequestsList")]
        public void Resolve_a_not_found_handler_for_an_unrecognised_request(HttpMethod method, string relativePath)
        {
            var requestHandler = _requestHandlerResolver.Resolve(method, relativePath);

            Assert.That(requestHandler, Is.TypeOf<NotFoundHandler>());
        }

        static object[] UnrecognizedRequestsList =
        {
            new object[] { HttpMethod.Put, "puppies/"},
            new object[] { HttpMethod.Get, KITTENS_ROOT },
            new object[] { HttpMethod.Post, KITTENS_RETRIEVE }
        };
    
    }


}