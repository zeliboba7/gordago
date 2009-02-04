/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;

namespace Gordago.Analysis.Vm {

  public class VmEvaluator {

    private static object[] _stack = new object[128];

    public static object Evaluate(VmCommand[] code, Analyzer analyzer) {
      int depth = -1;
      for (int i = 0; i < code.Length; i++) {
        VmCommand c = code[i];
        switch (c.Opcode) {
          case VmOpcode.Ink:
            int count = (int)c.Value;
            depth -= count - 1;
            if (c.CacheItem == null) {
              object[] parameters = new object[count];
              Array.Copy(_stack, depth, parameters, 0, count);
              _stack[depth] = analyzer.Compute(c.Function, parameters);
              CacheItem cacheItem = null;
              analyzer.Cache.TryGetValue(c.Function, parameters, out cacheItem);
              c.CacheItem = cacheItem;
            } else {
              _stack[depth] = analyzer.Compute(c.CacheItem);
            }
            if (_stack[depth] == null)
              return null;
            break;

          case VmOpcode.Lv:
            _stack[++depth] = c.Value;
            break;
          case VmOpcode.Le:
            if (c.Value != null) {
              IVector vector = _stack[depth] as IVector;
              int index = vector.Count - 1 + (int)c.Value;
              if (index < 0 || index >= vector.Count)
                return null;
              _stack[depth] = vector[index];
            } else {
              --depth;
              IVector vector = _stack[depth] as IVector;
              int index = vector.Count - 1 + (int)_stack[depth + 1];
              if (index < 0 || index >= vector.Count)
                return null;
              _stack[depth] = vector[index];
            }
            break;

          case VmOpcode.Ct:
            _stack[depth] = (int)(float)_stack[depth];
            break;

          case VmOpcode.Ctf:
            _stack[depth] = (float)(int)_stack[depth];
            break;

          case VmOpcode.Ad:
            --depth;
            _stack[depth] = (int)_stack[depth] + (int)_stack[depth + 1];
            break;
          case VmOpcode.Adf:
            --depth;
            _stack[depth] = (float)_stack[depth] + (float)_stack[depth + 1];
            break;
          case VmOpcode.Sb:
            --depth;
            _stack[depth] = (int)_stack[depth] - (int)_stack[depth + 1];
            break;
          case VmOpcode.Sbf:
            --depth;
            _stack[depth] = (float)_stack[depth] - (float)_stack[depth + 1];
            break;
          case VmOpcode.Ml:
            --depth;
            _stack[depth] = (int)_stack[depth] * (int)_stack[depth + 1];
            break;
          case VmOpcode.Mlf:
            --depth;
            _stack[depth] = (float)_stack[depth] * (float)_stack[depth + 1];
            break;
          case VmOpcode.Dv:
            --depth;
            _stack[depth] = (int)_stack[depth] / (int)_stack[depth + 1];
            break;
          case VmOpcode.Dvf:
            --depth;
            _stack[depth] = (float)_stack[depth] / (float)_stack[depth + 1];
            break;

          case VmOpcode.Nt:
            _stack[depth] = !(bool)_stack[depth];
            break;
          case VmOpcode.And:
            --depth;
            _stack[depth] = (bool)_stack[depth] && (bool)_stack[depth + 1];
            break;
          case VmOpcode.Or:
            --depth;
            _stack[depth] = (bool)_stack[depth] || (bool)_stack[depth + 1];
            break;
          case VmOpcode.Cq:
            --depth;
            _stack[depth] = (int)_stack[depth] == (int)_stack[depth + 1];
            break;
          case VmOpcode.Cqf:
            --depth;
            _stack[depth] = (float)_stack[depth] == (float)_stack[depth + 1];
            break;
          case VmOpcode.Cne:
            --depth;
            _stack[depth] = (int)_stack[depth] != (int)_stack[depth + 1];
            break;
          case VmOpcode.Cnef:
            --depth;
            _stack[depth] = (float)_stack[depth] != (float)_stack[depth + 1];
            break;
          case VmOpcode.Cgr:
            --depth;
            _stack[depth] = (int)_stack[depth] > (int)_stack[depth + 1];
            break;
          case VmOpcode.Cgrf:
            --depth;
            _stack[depth] = (float)_stack[depth] > (float)_stack[depth + 1];
            break;
          case VmOpcode.Cls:
            --depth;
            _stack[depth] = (int)_stack[depth] < (int)_stack[depth + 1];
            break;
          case VmOpcode.Clsf:
            --depth;
            _stack[depth] = (float)_stack[depth] < (float)_stack[depth + 1];
            break;
          case VmOpcode.Cge:
            --depth;
            _stack[depth] = (int)_stack[depth] >= (int)_stack[depth + 1];
            break;
          case VmOpcode.Cgef:
            --depth;
            _stack[depth] = (float)_stack[depth] >= (float)_stack[depth + 1];
            break;
          case VmOpcode.Cle:
            --depth;
            _stack[depth] = (int)_stack[depth] <= (int)_stack[depth + 1];
            break;

          case VmOpcode.Clef:
            --depth;
            _stack[depth] = (float)_stack[depth] <= (float)_stack[depth + 1];
            break;
        }
      }
      return _stack[depth];
    }

  }
}
