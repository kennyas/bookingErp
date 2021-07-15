# Reporting Service
The reporting service helps to process reporting data stored in a flat table structure that has been optimised for OLAP transaction queries. The service also includes a realtime dashboard system that broadcasts key metrics to multiple connected clients using Azure Signal service.

  

# Input

**Booking Service Events**

1.  BookingCreatedIntegrationEvent (event dispatched when a new booking is created either by a busboy or customer)
2.  BookingCancelledIntegrationEvent (event dispatched )
3.  BookingCompletedIntegrationEvent (event dispatched when a customer's booking has been paid for)

  
The reporting service subscribes to the above-mentioned events and heavily relies on them as its primary data source. The data derived from those events are stored in a flat table called CustomerBookingReport.

  

# Processing

**Background data sharding job**

The background job is responsible for performing intermittent data shading operations on data found in CustomerBookingReport table. The data is further analysed and stored in other tables to help reduce the performance overhead when running complex SQL report and dashboard generation queries. This just is run at the end of every day to capture the transactions of a particular day.

  

# Output

**Realtime Dashboard Service hosted on Azure SignalR Service**

This is a service that broadcasts dashboard data to connected clients using dedicated Hubs. incoming changes to bookings and sales data are broadcasted to all the connected clients.

 

**Reporting REST endpoints**

This is an ASP MVC core project that provides various endpoints to generate reports based on the provided parameters. Kindly note that the records returned is T-1 and not realtime. This is due to the volatility of transaction data changing during business hours which might negatively impact the correctness of the report data.