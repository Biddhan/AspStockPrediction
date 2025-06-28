using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using StockPredictionAPI.Models;

namespace StockPredictionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredictionController : ControllerBase, IDisposable
    {
        private readonly InferenceSession _modelSession;
        private readonly InferenceSession _scalerSession;
        private bool _disposed = false;

        public PredictionController()
        {
            // Loading ONNX models
            _scalerSession = new InferenceSession("MlModels/scaler.onnx");
            _modelSession = new InferenceSession("MlModels/linear_regression_model.onnx");
        }
    }
}