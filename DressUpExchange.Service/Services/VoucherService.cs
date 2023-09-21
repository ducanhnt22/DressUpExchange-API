using DressUpExchange.Service.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.Services
{
    public interface IVourcherService
    {
        Task<bool> CreateNewVoucher(int productID, VoucherRequest voucherRequest);
        Task<bool> UpdateVoucher(int productID, UpdateVoucherRequest updateVoucherRequest);
        Task<bool> DeleteVoucher(int voucherID);
        Task<bool> SaveVoucherByID(int voucherId, int userID);
    }
    public class VoucherService 
    {
    }
}
