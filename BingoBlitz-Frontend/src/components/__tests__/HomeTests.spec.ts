import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import HomeView from '@/views/HomeView.vue'

describe('SameHeight', () => {

  it('BoxesAreSameHeight', () => {
    const wrapper = mount(HomeView);

    let boxes = wrapper.findAll('.box');

    boxes.forEach((box) => {
      expect(box.element.clientHeight).toBe(boxes[0].element.clientHeight);
    });
  });

})