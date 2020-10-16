using System;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService
    {
        private readonly ISelectInvoiceService _selectInvoiceService;
        private readonly IConfidentialInvoiceService _confidentialInvoiceWebService;
        private readonly IBusinessLoansService _businessLoansService;

        public ProductApplicationService(ISelectInvoiceService selectInvoiceService, IConfidentialInvoiceService confidentialInvoiceWebService, IBusinessLoansService businessLoansService)
        {
            _selectInvoiceService = selectInvoiceService;
            _confidentialInvoiceWebService = confidentialInvoiceWebService;
            _businessLoansService = businessLoansService;
        }

        public int SubmitApplicationFor(ISellerApplication application)
        {
            if(application == null)
            {
                throw new ArgumentNullException("Application should have a value");
            }
            if(application.CompanyData == null)
            {
                throw new ArgumentNullException("CompanyData should have a value");
            }

            switch (application.Product)
            {
                case SelectiveInvoiceDiscount sid:
                    return _selectInvoiceService.SubmitApplicationFor(application.CompanyData.Number.ToString(), sid.InvoiceAmount, sid.AdvancePercentage);

                case ConfidentialInvoiceDiscount cid:
                    {
                        var result = _confidentialInvoiceWebService.SubmitApplicationFor(
                     new CompanyDataRequest
                     {
                         CompanyFounded = application.CompanyData.Founded,
                         CompanyNumber = application.CompanyData.Number,
                         CompanyName = application.CompanyData.Name,
                         DirectorName = application.CompanyData.DirectorName
                     }, cid.TotalLedgerNetworth, cid.AdvancePercentage, cid.VatRate);

                        return (result.Success) ? result.ApplicationId ?? -1 : -1;
                    }

                case BusinessLoans loans:
                    {
                        var result = _businessLoansService.SubmitApplicationFor(new CompanyDataRequest
                        {
                            CompanyFounded = application.CompanyData.Founded,
                            CompanyNumber = application.CompanyData.Number,
                            CompanyName = application.CompanyData.Name,
                            DirectorName = application.CompanyData.DirectorName
                        }, new LoansRequest
                        {
                            InterestRatePerAnnum = loans.InterestRatePerAnnum,
                            LoanAmount = loans.LoanAmount
                        });
                        return (result.Success) ? result.ApplicationId ?? -1 : -1;
                    }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
