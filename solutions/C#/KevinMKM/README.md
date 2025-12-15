# Cinema Ticket Reservation – JWT Authentication

## Run
export JWT__SigningKey="very-long-secure-random-secret"
dotnet run

## Test Users
admin / Admin123!
user / User123!

## Authorization
- POST /api/tickets/reserve -> tickets:reserve
- POST /api/screenings -> Admin + screenings:write
