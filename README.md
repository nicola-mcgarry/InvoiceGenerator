# Technical Test Requirement

Whenever the sales department confirms an order, a request must be sent to the accounts department to raise an invoice for the items on the order. Currently, these requests are made in an informal, unstructured way, and the accounts department often need to request additional information from the sales person about the order to be invoiced.

The JSON below contains all the data required by the accounts department so that they can raise an invoice.

The requirement is to create a small .NET web application that generates an invoice request for the accounts department as a PDF file, using the JSON as a data source. The source code for the application should be made available as a zip file or hosted on a public repository e.g., GitHub at least 48 hours before the second interview.

## JSON Data

```json
{
    "request": [
        {
            "order_id": 62654,
            "sales_person": "Jed Blackthorn",
            "order_confirmed_date": "2022-12-16T16:51:00",
            "currency_name": "GBP",
            "special_instructions": null,
            "invoice_advertiser": "Alpha Inc",
            "invoice_company_name": "Alpha Inc",
            "invoice_address1": "40 Septon Drive",
            "invoice_address2": null,
            "invoice_address3": null,
            "invoice_city": "Portsmouth",
            "invoice_state_county": "Hampshire",
            "invoice_post_code": "SO23 4KL",
            "invoice_country_name": "United Kingdom",
            "invoice_contact_name": "Daenarys Baseley",
            "invoice_contact_email_address": "dbaseley@alphainc.com",
            "items": [
                {
                    "order_item_id": 52849,
                    "product_name": "Clinical Services Journal",
                    "purchase_order": "PO 566/21",
                    "item": "Quarter Page",
                    "month_name": "May",
                    "year": 2024,
                    "gross_price": 375.0,
                    "net_price": 375.0
                },
                {
                    "order_item_id": 52850,
                    "product_name": "Clinical Services Journal",
                    "purchase_order": "PO 566/21",
                    "item": "Quarter Page",
                    "month_name": "June",
                    "year": 2024,
                    "gross_price": 375.0,
                    "net_price": 375.0
                }
            ]
        }
    ]
}
