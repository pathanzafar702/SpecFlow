using System;
using FluentAssertions;
using Io.Cucumber.Messages;
using TechTalk.SpecFlow.CommonModels;
using TechTalk.SpecFlow.CucumberMessages;
using Xunit;

namespace TechTalk.SpecFlow.RuntimeTests.CucumberMessages
{
    public class CucumberMessageFactoryTests
    {
        [Fact(DisplayName = @"BuildTestRunResultMessage should return a TestRunResult message object")]
        public void BuildTestRunResultMessage_DateTime_ShouldReturnTestRunResultMessageObject()
        {
            // ARRANGE
            var cucumberMessageFactory = new CucumberMessageFactory();
            var dateTime = new DateTime(2019, 5, 9, 14, 27, 48, DateTimeKind.Utc);

            // ACT
            var actualTestRunStartedMessageResult = cucumberMessageFactory.BuildTestRunStartedMessage(dateTime);

            // ASSERT
            actualTestRunStartedMessageResult.Should().BeAssignableTo<ISuccess<TestRunStarted>>();
        }

        [Fact(DisplayName = @"BuildTestRunResultMessage should return a TestRunResult message object with the specified date and time")]
        public void BuildTestRunResultMessage_DateTime_ShouldReturnTestRunResultMessageObjectWithSpecifiedDateAndTime()
        {
            // ARRANGE
            var cucumberMessageFactory = new CucumberMessageFactory();
            var dateTime = new DateTime(2019, 5, 9, 14, 27, 48, DateTimeKind.Utc);

            // ACT
            var actualTestRunStartedMessageResult = cucumberMessageFactory.BuildTestRunStartedMessage(dateTime);

            // ASSERT
            actualTestRunStartedMessageResult.Should().BeAssignableTo<ISuccess<TestRunStarted>>()
                                             .Which.Result.Timestamp.ToDateTime().Should().Be(dateTime);
        }

        [Fact(DisplayName = @"BuildTestRunResultMessage should return a TestRunResult message object with SpecFlow as used Cucumber implementation")]
        public void BuildTestRunResultMessage_ValidParameters_ShouldReturnTestRunResultMessageObjectWithSpecFlowAsUsedCucumberImplementation()
        {
            // ARRANGE
            const string expectedCucumberImplementation = @"SpecFlow";
            var cucumberMessageFactory = new CucumberMessageFactory();
            var dateTime = new DateTime(2019, 5, 9, 14, 27, 48, DateTimeKind.Utc);

            // ACT
            var actualTestRunStartedMessageResult = cucumberMessageFactory.BuildTestRunStartedMessage(dateTime);

            // ASSERT

            actualTestRunStartedMessageResult.Should().BeAssignableTo<ISuccess<TestRunStarted>>()
                                             .Which.Result.CucumberImplementation.Should().Be(expectedCucumberImplementation);
        }

        [Theory(DisplayName = @"BuildTestCaseStarted should return a failure when a non-UTC date has been specified")]
        [InlineData(DateTimeKind.Local)]
        [InlineData(DateTimeKind.Unspecified)]
        public void BuildTestRunResultMessage_NonUtcDate_ShouldReturnFailure(DateTimeKind dateTimeKind)
        {
            // ARRANGE
            var cucumberMessageFactory = new CucumberMessageFactory();
            var dateTime = new DateTime(2019, 5, 9, 14, 27, 48, dateTimeKind);
            var pickleId = Guid.NewGuid();

            // ACT
            var result = cucumberMessageFactory.BuildTestCaseStartedMessage(pickleId, dateTime);

            // ASSERT
            result.Should().BeAssignableTo<IFailure>();
        }

        [Fact(DisplayName = @"BuildTestCaseStarted should return a message with the correct pickle ID")]
        public void BuildTestCaseStarted_ValidData_ShouldReturnMessageWithCorrectPickleId()
        {
            // ARRANGE
            var cucumberMessageFactory = new CucumberMessageFactory();
            var dateTime = new DateTime(2019, 5, 9, 14, 27, 48, DateTimeKind.Utc);
            var pickleId = Guid.NewGuid();

            // ACT
            var result = cucumberMessageFactory.BuildTestCaseStartedMessage(pickleId, dateTime);

            // ASSERT
            result.Should().BeAssignableTo<ISuccess<TestCaseStarted>>().Which
                  .Result.PickleId.Should().Be(pickleId.ToString("D"));
        }

        [Fact(DisplayName = @"BuildTestCaseStarted should return a success when a UTC date has been specified")]
        public void BuildTestCaseStarted_UtcDate_ShouldReturnSuccess()
        {
            // ARRANGE
            var cucumberMessageFactory = new CucumberMessageFactory();
            var dateTime = new DateTime(2019, 5, 9, 14, 27, 48, DateTimeKind.Utc);
            var pickleId = Guid.NewGuid();

            // ACT
            var result = cucumberMessageFactory.BuildTestCaseStartedMessage(pickleId, dateTime);

            // ASSERT
            result.Should().BeAssignableTo<ISuccess<TestCaseStarted>>();
        }
    }
}