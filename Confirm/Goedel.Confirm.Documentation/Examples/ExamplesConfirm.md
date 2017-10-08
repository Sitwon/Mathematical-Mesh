
##Check service connection

It is often useful to be able to verify that a service is ready and willing
to perform transactions before attempting to perform one. Especially so
when the transaction requires considerable amounts of data and may require the 
use of specific server determined authentication options.

The request message is 'HelloRequest' and has no parameters:


~~~~
POST /.well-known/confirm/HTTP/1.1
Host: prismproof.org
Content-Length: 23

{
  "HelloRequest": {}}
~~~~


The response message specifies the protocol version(s) supported, the corresponding
encodings and bindings:


~~~~
HTTP/1.1 200 OK
Date: Thu 11 May 2017 03:12:20
Content-Length: 157

{
  "HelloResponse": {
    "Status": 201,
    "StatusDescription": "Operation completed successfully",
    "Version": {
      "Major": 0,
      "Minor": 1}}}
~~~~



##Enquirer posts request.

Alice attempts to log into her computer system as administrator. The site policy 
requires that all administrative logins be confirmed via Mesh/Confirm. Alice's
confirmation account is alice@example.com. In this exchange, the computer Alice 
is trying to access is the Enquirer, the device she uses to respond to the query 
(her watch) is the Responder and the Mesh/Confirm service at example.com is the 
broker.

The computer acting as Enquirer (example.net) creates a confirmation request as 
follows:

~~~~
<?xml version="1.0" encoding="utf-8" ?>
<srml xmlns="http://hallambaker.com/Schemas/srml.xsd">
  <h1>Grant Administrator</h1>
  <p>Host: example.net</p>
  <button value="Access">Access</button>
</srml>
~~~~

Note that it is not necessary to specify the reject option since this is 
implicit.

The request message is 'EnquireRequest' which contains an object signed by the
Enquirer that specifies the account the request is directed to and the 
request text. 


~~~~
POST /.well-known/confirm/HTTP/1.1
Host: prismproof.org
Content-Length: 25

{
  "EnquireRequest": {}}
~~~~


The request is accepted and a success response returned specifying the transaction ID.


~~~~
HTTP/1.1 200 OK
Date: Thu 11 May 2017 03:12:20
Content-Length: 105

{
  "EnquireResponse": {
    "Status": 201,
    "StatusDescription": "Operation completed successfully"}}
~~~~


Note that while the requirement that request messages be authenticated by means
of a digital signature is within the scope of Mesh/Recrypt, the specification of
the filtering rules is not.

##Responder fetches pending requests.

At present, the only mechanism for determining if there are pending requests is by
polling. Provision of a push notification mechanism is an obvious priority for
future improvement of the protocol.

Alice's watch regularly polls the broker to determine if there are pending confirmation
requests.



~~~~
POST /.well-known/confirm/HTTP/1.1
Host: prismproof.org
Content-Length: 25

{
  "PendingRequest": {}}
~~~~


The response message contains the number of pending requests meeting the selection 
criteria and the returned requests.


~~~~
HTTP/1.1 200 OK
Date: Thu 11 May 2017 03:12:20
Content-Length: 105

{
  "PendingResponse": {
    "Status": 201,
    "StatusDescription": "Operation completed successfully"}}
~~~~


Each request is signed by the Enquirer that originally generated it.

##Responder replies to request.

Alice selects the pending access request and grants herself access to the machine
she is attempting to log in to. Her watch creates a signed response message 
containing the digest of the original request and her response "Accept".



~~~~
POST /.well-known/confirm/HTTP/1.1
Host: prismproof.org
Content-Length: 25

{
  "RespondRequest": {}}
~~~~


The response message tells Alice that the transaction completed successfully and
the broker has her acceptance message.


~~~~
HTTP/1.1 200 OK
Date: Thu 11 May 2017 03:12:20
Content-Length: 105

{
  "RespondResponse": {
    "Status": 201,
    "StatusDescription": "Operation completed successfully"}}
~~~~


Note that in future versions of the protocol it may be desirable to make use of
additional affordances of the device such as the ability to perform biometric 
capture such as fingerprint or facial recognition.

Another possibility is that the user might be asked to enter a one time use PIN 
generated by the Enquirer, thus verifying that the user is indeed responding 
to the confirmation request they believe they are responding to.

##Enquirer fetches result

The enquirer obtains the result of the confirmation request by polling using the 
Status transaction.



~~~~
POST /.well-known/confirm/HTTP/1.1
Host: prismproof.org
Content-Length: 24

{
  "StatusRequest": {}}
~~~~


Since Alice has responded, the response message contains the signed result:


~~~~
HTTP/1.1 200 OK
Date: Thu 11 May 2017 03:12:20
Content-Length: 104

{
  "StatusResponse": {
    "Status": 201,
    "StatusDescription": "Operation completed successfully"}}
~~~~


As with the use of polling on the user side, it is obviously desirable to
eliminate the need for polling by introducing a callback registration 
mechanism. 

