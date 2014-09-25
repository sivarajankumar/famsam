angular.module('iqdService', [])
    .service('ImageQualityDetector', function () {
        var _this = this;
        this.canvas = document.createElement('canvas');
        this.tmpCtx = document.createElement('canvas').getContext('2d');

        this.getCanvas = function(w,h) {
            var c = _this.canvas;
            c.width = w;
            c.height = h;
            return c;
        };

        /**
         * Get pixel of an image.
         * @param imgSrc available src of an image.
         * @returns {ImageData}
         */
        this.getPixels = function(imgSrc) {
            var img = document.createElement('img');
            img.src=imgSrc;
            var c = _this.getCanvas(img.width, img.height);
            var ctx = c.getContext('2d');
            ctx.drawImage(img, 0, 0);
            return ctx.getImageData(0,0,c.width,c.height);
        };



        this.createImageData = function (w, h){
            return _this.tmpCtx.createImageData(w, h);
        };

        this.deBlur = function(pixels, weights, opaque) {
            var side = Math.round(Math.sqrt(weights.length));
            var halfSide = Math.floor(side/2);
            var src = pixels.data;
            var sw = pixels.width;
            var sh = pixels.height;
            // pad output by the convolution matrix
            var w = sw;
            var h = sh;
            var output = _this.createImageData(w, h);
            var dst = output.data;
            // go through the destination image pixels
            var alphaFac = opaque ? 1 : 0;
            for (var y=0; y<h; y++) {
                for (var x=0; x<w; x++) {
                    var sy = y;
                    var sx = x;
                    var dstOff = (y*w+x)*4;
                    // calculate the weighed sum of the source image pixels that
                    // fall under the convolution matrix
                    var r=0, g=0, b=0, a=0;
                    for (var cy=0; cy<side; cy++) {
                        for (var cx=0; cx<side; cx++) {
                            var scy = sy + cy - halfSide;
                            var scx = sx + cx - halfSide;
                            if (scy >= 0 && scy < sh && scx >= 0 && scx < sw) {
                                var srcOff = (scy*sw+scx)*4;
                                var wt = weights[cy*side+cx];
                                r += src[srcOff] * wt;
                                g += src[srcOff+1] * wt;
                                b += src[srcOff+2] * wt;
                                a += src[srcOff+3] * wt;
                            }
                        }
                    }
                    dst[dstOff] = r;
                    dst[dstOff+1] = g;
                    dst[dstOff+2] = b;
                    dst[dstOff+3] = a + alphaFac*(255-a);
                }
            }

            _this.tmpCtx.putImageData(output,0,0);
            return _this.canvas.toDataURL();
            //return output;
        };
    });