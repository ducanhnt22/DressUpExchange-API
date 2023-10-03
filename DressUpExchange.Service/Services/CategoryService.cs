using AutoMapper;
using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DressUpExchange.Service.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateCategory(CategoryRequest request);
        Task<CategoryResponse> GetCategoryById(int id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Category Invalid!!!", "");
                }

                var category = _mapper.Map<Category>(request);

                await _unitOfWork.Repository<Category>().CreateAsync(category);
                await _unitOfWork.CommitAsync();

                var categoryResponse = _mapper.Map<CategoryResponse>(category);
                return categoryResponse;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Create Category Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<CategoryResponse> GetCategoryById(int id)
        {
            try
            {
                var category = await _unitOfWork.Repository<Category>().GetById(id);

                if (category == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Category not found", "");
                }

                var categoryResponse = _mapper.Map<CategoryResponse>(category);
                return categoryResponse;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Category Error!!!", ex.InnerException?.Message);
            }
        }
    }

}
