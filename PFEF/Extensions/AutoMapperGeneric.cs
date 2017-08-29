using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFEF.Extensions
{
    public class AutoMapperGeneric<TSource, TDestination>
        {
            public static TDestination ConvertToDBEntity(TSource model)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<TSource, TDestination>();
                });
                IMapper mapper = config.CreateMapper();
                var ContMapeado = mapper.Map<TSource, TDestination>(model);
                return ContMapeado;
                }
       }
    
}