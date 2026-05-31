Pre start evaluation:
---------------------

The PaymentService class is doing too much. Need to split out:
	Reading from application configuration (ConfigurationManager.AppSettings).
	Deciding which database implementation to instantiate.
	Executing business rules and validation logic (the switch statement).
	Orchestrating the account balance update and saving it.


I don't like the use of switches, feels like it violates OCP. Unsure how i would rewrite. come to this last.

cannot unit test PaymentService in isolation. IE. If you write a test for MakePayment, it will try to connect to the real database and read the real configuration file.

uses lower-level implementations (ConfigurationManager, AccountDataStore, BackupAccountDataStore). It uses the new() keyword to create its own dependencies inside the methods.


Notes During:
---------------------

I'm much slower at writing tests than coding/refactoring as i'm still somewhat new to writing tests. So i'm going to ignore the advice given in the email and write the tests last as i am time contsrained.

Switches: <br>
- evaluate what each switch is doing
- write the validation logic in boolean expressions.
- give each own validation class
- hooked these to a dictionary in a new PaymentValidatorFactory so PaymentService doesn't need to know about the concrete classes

Dependency Injection:<br>
- stripped out all the 'new' keywords from PaymentService.
- injected IAccountDataStore and IValidatorFactory via the constructor.
- This lets me mock the database and test the service in isolation.


Configuration & DataStore logic:<br>
- PaymentService reading AppSettings is a SRP violation.
- moved the 'Live vs Backup' check into a dedicated AccountDataStoreFactory.
- keeps the original config requirement intact but gets it out of the orchestration layer.
- had to remember to make the existing data stores inherit from IAccountDataStore so the factory works.

Domain Model (Account):<br>
- noticed PaymentService was reaching in and doing math directly on account.Balance.
- moved this into the Account class itself (account.DeductBalance). Better encapsulation / avoids anemic domain model.

Tests (Doing these last):
- used Moq to fake the factory and data store.
- setup a Builder pattern (MakePaymentRequestBuilder, AccountBuilder) to handle the test data.
- saves me from writing object initializations in every single test method.


Post Evaluation:
------------------------
Tests still needed:<br>
- The validators (BacsPaymentValidator, FasterPaymentsValidator, ChapsPaymentValidator). I need to write tests for these to verify their specific boolean rules and edge cases.
 Account domain model tests. Since I moved DeductBalance into the Account class, I need a quick test to prove the math works and it doesn't allow weird states (like deducting negative amounts).
 AccountDataStoreFactory tests. Need to prove it actually reads the "Backup" vs "Live" string and returns the correct instance type.

Nice to haves / Future improvements:<br>
- Logging. We are dealing with payments; returning a silent 'false' on failure is a bit risky. Would be great to inject an ILogger to record exactly why a payment failed.
 Richer return types. MakePaymentResult currently just flips a boolean. It would be much better to include an error message or enum so the caller knows why it failed.
 Exception handling. If the database drops connection during UpdateAccount, it's going to blow up. A try/catch block at the orchestration level or in the data access layer would fix this.

