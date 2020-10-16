using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using System;
using System.Collections.Generic;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests
    {
        private readonly ProductApplicationService _productApplicationServicePositive;
        private readonly ProductApplicationService _productApplicationServiceNegative;
        public ProductApplicationTests()
        {
            var mockedExternalServicePositive = new MockedExternalService(true);
            var mockedExternalServiceNegative = new MockedExternalService(false);
            _productApplicationServicePositive = new ProductApplicationService(mockedExternalServicePositive, mockedExternalServicePositive, mockedExternalServicePositive);
            _productApplicationServiceNegative = new ProductApplicationService(mockedExternalServiceNegative, mockedExternalServiceNegative, mockedExternalServiceNegative);
        }

        [Fact]
        public void NullReferenceExceptionApplicationTest()
        {
            Assert.Throws<ArgumentNullException>(() => _productApplicationServicePositive.SubmitApplicationFor(null));
        }

        [Fact]
        public void NullReferenceExceptionCompanyTest()
        {
            Assert.Throws<ArgumentNullException>(() => _productApplicationServicePositive.SubmitApplicationFor(new SellerApplication()));
        }

        [Fact]
        public void CorrectServiceExecutedSelectiveTest()
        {
            var application = new SellerApplication
            {
                CompanyData = new SellerCompanyData(),
                Product = new SelectiveInvoiceDiscount()
            };
            Assert.Equal(123, _productApplicationServicePositive.SubmitApplicationFor(application));
        }
        [Fact]
        public void CorrectServiceExecutedLoansTest()
        {
            var application = new SellerApplication
            {
                CompanyData = new SellerCompanyData(),
                Product = new BusinessLoans()
            };
            Assert.Equal(12345, _productApplicationServicePositive.SubmitApplicationFor(application));
        }
        [Fact]
        public void CorrectServiceExecutedConfidentialTest()
        {
            var application = new SellerApplication
            {
                CompanyData = new SellerCompanyData(),
                Product = new ConfidentialInvoiceDiscount()
            };
            Assert.Equal(1234, _productApplicationServicePositive.SubmitApplicationFor(application));
        }

        [Fact]
        public void CorrectServiceExecutedConfidentialNegativeTest()
        {
            var application = new SellerApplication
            {
                CompanyData = new SellerCompanyData(),
                Product = new ConfidentialInvoiceDiscount()
            };
            Assert.Equal(-1, _productApplicationServiceNegative.SubmitApplicationFor(application));
        }
        [Fact]
        public void CorrectServiceExecutedSelectiveNegativeTest()
        {
            var application = new SellerApplication
            {
                CompanyData = new SellerCompanyData(),
                Product = new SelectiveInvoiceDiscount()
            };
            Assert.Equal(-1, _productApplicationServiceNegative.SubmitApplicationFor(application));
        }
        [Fact]
        public void CorrectServiceExecutedLoanNegativeTest()
        {
            var application = new SellerApplication
            {
                CompanyData = new SellerCompanyData(),
                Product = new BusinessLoans()
            };
            Assert.Equal(-1, _productApplicationServiceNegative.SubmitApplicationFor(application));
        }
    }
    public class MockedExternalService : IBusinessLoansService, IConfidentialInvoiceService, ISelectInvoiceService
    {
        private readonly bool _result;
        public MockedExternalService(bool result)
        {
            _result = result;
        }
        public IApplicationResult SubmitApplicationFor(CompanyDataRequest applicantData, LoansRequest businessLoans)
        {
            return new MockedResult { Success = _result, ApplicationId = _result ? 12345 : -1 };
        }

        public IApplicationResult SubmitApplicationFor(CompanyDataRequest applicantData, decimal invoiceLedgerTotalValue, decimal advantagePercentage, decimal vatRate)
        {
            return new MockedResult {Success = _result, ApplicationId = _result ? 1234 : -1 };
        }

        public int SubmitApplicationFor(string companyNumber, decimal invoiceAmount, decimal advancePercentage)
        {
            return _result ? 123 : -1;
        }
    }
    public class MockedResult : IApplicationResult
    {
        public int? ApplicationId { get; set; }
        public bool Success { get; set; }
        public IList<string> Errors { get; set; }
    }
}
