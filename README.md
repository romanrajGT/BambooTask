## 1. Plugin for Discount Requirement
1. Plugin Name - **Nop.Plugin.Custom.Discounts**
2. Installation - The source code is in the **Plugins** folder, and with that folder in **src/Plugins**, log in to **Admin Panel > Local Plugins** and find the plugin in the list and install it.
3. With this plugin installed, to enable and apply discount percentage;
    1. Go to **Admin Panel > Promotions > Discounts > Add new**
    2. Check or uncheck **Is active** checkbox for enabling or disabling the discount
    3. Add **Name** for discount, set **Discount type** to **Assigned to order total**
    4. Check **Use Percentage** and enter 10 in **Discount Percentage** field for applying 10% discount and click **Save and Continue Edit** button
    5. Now down in **Requirements** panel for the same Discount, select **Custom Discounts** from the **Discount Requirement Type** dropdown and click **Save** button to apply the Requirement Rule of customer to have 3 or more orders previously to be eligible for the discount.

##2. Modify Checkout for Gift Message
1. Go to **Admin Panel > Catalog > Attributes > Checkout Attributes**
2. Click “Add new”
3. Enter field values:
    * Name: Gift Message
    * Text prompt: Enter a gift message (optional)
    * Control type: Multiline Textbox
    * Required: Leave unchecked (optional)
    * Display order: (Set as needed)

NopCommerce will automatically:
    * Add it to the checkout UI.
    * Save the value with the order.
    * Show it in the Admin Panel > Orders > Order Details page.
    * Show it in the customer's My Account > Orders section.


3. Add Search by Name in Product Attribute list in Admin Panel
1. All changes done in the core code in files.


4. Order Retrieval API Plugin
1. Plugin Name - Nop.Plugin.Api.OrderRetrieval
2. Installation - The source code is in the Plugins folder, and with that folder in src/Plugins, log in to Admin Panel > Local Plugins and find the plugin in the list and install it. 

5. Test Order Retrieval APIs
1. You can find postman collection in https://github.com/romanrajGT/BambooTask/blob/main/Bamboo.postman_collection.json Import it into your Postman to test API endpoints, such as authentication & order retrieval. 
    1. Generate JWT Token:
        * In Postman, create a new http POST request to baseUrl/api/token/generate with username in the post body request as shown in the postman collection
    2. Testing the order retrieval API: Do a get request to baseUrl/api/order/by-email?email=yourCustomerEmail, yourCustomerEmail being the email of the customer of whom the order details are to be retrieved. Do not forget to pass Bearer token generated from Token Generation endpoint from first point with the request. 

6. 
