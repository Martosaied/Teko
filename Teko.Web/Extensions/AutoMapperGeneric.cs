using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Teko.Model;
using Teko.ViewModels;

namespace Teko.Extensions
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
    public class ContenidoMapper
    {
        public static Contenidos ConvertVMtoContenido(dynamic SVM)
        {
            Contenidos ContenidoMapeado = new Contenidos();
            ContenidoMapeado.Nombre = SVM.Nombre;
            ContenidoMapeado.Descripcion = SVM.Descripcion;
            ContenidoMapeado.EscuelasId = SVM.EscuelasId;
            ContenidoMapeado.MateriasId = SVM.MateriasId;
            ContenidoMapeado.TiposContenidosId = SVM.TiposContenidosId;
            ContenidoMapeado.Profesor = SVM.Profesor;
            ContenidoMapeado.Cursada = Convert.ToInt32(SVM.Cursada);
            return ContenidoMapeado;
        }
    }
    
}