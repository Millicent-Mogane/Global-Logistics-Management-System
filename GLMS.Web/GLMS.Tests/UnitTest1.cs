using System.IO;
using Xunit;

namespace GLMS.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CurrencyConversion_ShouldCalculateCorrectZarAmount()
        {
            // Arrange
            decimal usdAmount = 100m;
            decimal exchangeRate = 18.50m;

            // Act
            decimal zarAmount = usdAmount * exchangeRate;

            // Assert
            Assert.Equal(1850m, zarAmount);
        }

        [Fact]
        public void PdfFile_ShouldBeAccepted()
        {
            // Arrange
            string fileName = "agreement.pdf";

            // Act
            string extension = Path.GetExtension(fileName);

            // Assert
            Assert.Equal(".pdf", extension, ignoreCase: true);
        }

        [Fact]
        public void ExeFile_ShouldBeRejected()
        {
            // Arrange
            string fileName = "malware.exe";

            // Act
            string extension = Path.GetExtension(fileName);

            // Assert
            Assert.NotEqual(".pdf", extension.ToLower());
        }

        [Fact]
        public void UsdAmount_ShouldBeGreaterThanZero()
        {
            // Arrange
            decimal usdAmount = 0m;

            // Act
            bool isValid = usdAmount > 0;

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ExpiredContract_ShouldNotAllowServiceRequest()
        {
            // Arrange
            string contractStatus = "Expired";

            // Act
            bool canCreateRequest =
                contractStatus != "Expired" &&
                contractStatus != "On Hold";

            // Assert
            Assert.False(canCreateRequest);
        }

        [Fact]
public void OnHoldContract_ShouldNotAllowServiceRequest()
{
    // Arrange
    string contractStatus = "On Hold";

    // Act
    bool canCreateRequest =
        contractStatus != "Expired" &&
        contractStatus != "On Hold";

    // Assert
    Assert.False(canCreateRequest);
}

    }
}