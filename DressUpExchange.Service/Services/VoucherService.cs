using AutoMapper;
using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using Microsoft.EntityFrameworkCore;
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
        
        Task<VoucherResponse> GetVoucherByProductID(int? customerID = null);

        public Task<VoucherResponse> GetVoucherByCustomerID(int? customerID = null);
    }
    public class VoucherService : IVourcherService
    {
        private static IUnitOfWork _unitOfWork;
        private static IMapper _mapper;

        public VoucherService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> CreateNewVoucher(int productID, VoucherRequest voucherRequest)
        {
            var newVoucher = _mapper.Map<Voucher>(voucherRequest);
            newVoucher.ProductId = productID;
           
           
            _unitOfWork.Repository<Voucher>().CreateAsync(newVoucher);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> DeleteVoucher(int voucherID)
        {
           var deleteObject =   _unitOfWork.Repository<Voucher>().Where(x => x.VoucherId == voucherID).FirstOrDefault();
         _unitOfWork.Repository<Voucher>().Delete(deleteObject);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public  async Task<VoucherResponse> GetVoucherByCustomerID(int? customerID = null)
        {
           var voucherList =_unitOfWork.Repository<Voucher>().Where(x => x.UserId == customerID).AsQueryable();
            List<VoucherDetailResponse> listVoucher = new List<VoucherDetailResponse>() ;
            foreach (var voucher in voucherList)
            {
                var vourcherDetail = _mapper.Map<VoucherDetailResponse>(voucher);
                listVoucher.Add(vourcherDetail);
            }
            VoucherResponse voucherDetailResponse = new VoucherResponse() {
               total = voucherList.Count(),
               vouchers = listVoucher.ToList()
            };

            return voucherDetailResponse;
            

        }

        public async Task<VoucherResponse> GetVoucherByProductID(int? productID = null)
        {
            var voucherList = _unitOfWork.Repository<Voucher>().Where(x => x.ProductId == productID).AsQueryable();
            List<VoucherDetailResponse> listVoucher = new List<VoucherDetailResponse>();
            foreach (var voucher in voucherList)
            {
                var vourcherDetail = _mapper.Map<VoucherDetailResponse>(voucher);
                listVoucher.Add(vourcherDetail);
            }
            VoucherResponse voucherDetailResponse = new VoucherResponse()
            {
                total = voucherList.Count(),
                vouchers = listVoucher.ToList()
            };

            return voucherDetailResponse;
        }

        public async Task<bool> SaveVoucherByID(int voucherId, int userID)
        {
            UserSavedVoucher saveVoucherByID = new UserSavedVoucher()
            {
                VoucherId = voucherId,
                UserId = userID
                ,Status = SaveVoucherState.InSave.ToString() 
            };
         await   _unitOfWork.Repository<UserSavedVoucher>().CreateAsync(saveVoucherByID);
           _unitOfWork.Commit();
            return true;
        }

        public async Task<bool> UpdateVoucher(int productID, UpdateVoucherRequest updateVoucherRequest)
        {
            var updateVoucherFind =await _unitOfWork.Repository<Voucher>().FindAsync(x => x.ProductId == productID);
            if (updateVoucherFind is null)
            {
                return false;
            }
            updateVoucherFind.ExpireDate = updateVoucherRequest.newExpireDate;
            updateVoucherFind.Code = updateVoucherRequest.newCode;
            updateVoucherFind.DiscountAmount = updateVoucherRequest.newDismountAmount;
            updateVoucherFind.RemainingCount = updateVoucherRequest.newTotal;
            updateVoucherFind.ExpireDate = updateVoucherRequest.newExpireDate;
         await   _unitOfWork.Repository<Voucher>().Update(updateVoucherFind,updateVoucherFind.VoucherId);
            await _unitOfWork.CommitAsync();

            return true;

        }

       // public async Task<int> GetVoucherId() => (_unitOfWork.Repository<Voucher>().GetAll().OrderByDescending(x => x.VoucherId).FirstOrDefault()?.VoucherId ?? 0)+1  ;
    }
}

