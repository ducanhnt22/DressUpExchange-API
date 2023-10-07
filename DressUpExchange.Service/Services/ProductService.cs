using AutoMapper;
using AutoMapper.QueryableExtensions;
using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Exceptions;
using DressUpExchange.Service.Helpers;
using DressUpExchange.Service.Ultilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.Services
{
    public interface IProductService
    {
        Task<ProductResponseGeneral> GetProducts(ProductGetRequest request);
        Task<ProductResponse> DeleteProduct(int id);
        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<ProductResponse> GetProductById(int id);
        Task<ProductResponse> UpdateProduct(int id, ProductRequest request);
    }
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductResponse> CreateProduct(ProductRequest product)
        {
            try
            {
                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Product Invalid!!!", "");
                }

                var p = _mapper.Map<Product>(product);
                p.CategoryId = product.CategoryId;

                await _unitOfWork.Repository<Product>().CreateAsync(p);
                await _unitOfWork.CommitAsync();

                if (product.Images != null && product.Images.Any())
                {
                    foreach (var imageRequest in product.Images)
                    {
                        var productImage = _mapper.Map<ProductImage>(imageRequest);
                        productImage.ProductId = p.ProductId;
                        await _unitOfWork.Repository<ProductImage>().CreateAsync(productImage);
                    }

                    await _unitOfWork.CommitAsync();
                }
                return _mapper.Map<Product, ProductResponse>(p);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Insert Product Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<ProductResponse> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.Repository<Product>().GetAsync(p => p.ProductId == id);

                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id", id.ToString());
                }

                _unitOfWork.Repository<Product>().Delete(product);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Delete Product Error!!!", ex.InnerException?.Message);
            }
        }
        public async Task<ProductResponse> GetProductById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Id Product Invalid", "");
                }

                var response = await _unitOfWork.Repository<Product>()
                    .Include(p => p.ProductImages)
                    .SingleOrDefaultAsync(p => p.ProductId == id);

                if (response == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id", "");
                }

                return _mapper.Map<ProductResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get Product By ID Error!!!", ex.InnerException?.Message);
            }
        }

        public async Task<ProductResponseGeneral> GetProducts(ProductGetRequest request)
        {
            try
            {
                var filter = _mapper.Map<ProductResponse>(request);
                var products = await QueryFormat.GetProductResponsesAsync(request);
                var filterLogs = products.ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                                    .DynamicFilter(filter)
                                    .ToList();
                int count = filterLogs.Count();
                var result = new ProductResponseGeneral()
                {
                    Count = count,
                    Items = filterLogs
                };

                /* var products1 = _unitOfWork.Repository<Product>()
                    .GetAll()
                    .Include(p => p.ProductImages)
                    .Include(p => p.Category)
                    .ProjectTo<ProductResponse>(_mapper.ConfigurationProvider)
                    .DynamicFilter(filter)
                    .ToList();*/
                /*     var sort = PageHelper<ProductResponse>.Sorting(paging.SortType, products, paging.ColName);
                     var result = PageHelper<ProductResponse>.Paging(sort, paging.Page, paging.PageSize);*/
                return result;
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get product list error!!!!!", ex.Message);
            }
        }

        public async Task<ProductResponse> UpdateProduct(int id, ProductRequest request)
        {
            try
            {
                Product product = await _unitOfWork.Repository<Product>()
                    .Include(p => p.ProductImages)
                    .SingleOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found product with id", id.ToString());
                }

                _mapper.Map<ProductRequest, Product>(request, product);

                if (request.Images != null && request.Images.Any())
                {
                    product.ProductImages.Clear();

                    foreach (var imageRequest in request.Images)
                    {
                        var productImage = _mapper.Map<ProductImage>(imageRequest);
                        product.ProductImages.Add(productImage);
                    }
                }

                await _unitOfWork.CommitAsync();
                return _mapper.Map<Product, ProductResponse>(product);
            }
            catch (CrudException ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update product error!!!!!", ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
